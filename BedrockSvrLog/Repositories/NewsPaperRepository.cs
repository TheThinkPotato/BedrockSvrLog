using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;
using BedrockSvrLog.Models;
using System.Runtime.CompilerServices;
using BedrockSvrLog.Helpers;

namespace BedrockSvrLog.Repositories;

public class NewsPaperRepository
{
    private readonly AppDbContext _context;
    public NewsPaperRepository(AppDbContext context)
    {
        _context = context;

    }

    public async Task<List<PlayerDeaths>> getPlayerDeathDetailsAsync(int? lastNumOfDays, int? take, CancellationToken ct)
    {
        var currentDate = DateTime.Now;
        var sevenDaysAgo = currentDate.AddDays(-lastNumOfDays ?? -7);

        var playerDeaths = await _context.PlayerDeaths
            .Include(pd => pd.Player)
            .Include(pd => pd.Killer)
            .Where(pd => pd.DeathTime > sevenDaysAgo)
            .OrderByDescending(pd => pd.DeathTime)
            .Take(take ?? 2)
            .ToListAsync(ct);

        return playerDeaths;
    }

    public async Task<List<PlayerKillArticleDto>> getPlayerKillDetailsAsync(int? lastNumOfDays, int? take, CancellationToken ct)
    {
        var currentDate = DateTime.Now;
        var sevenDaysAgo = currentDate.AddDays(-lastNumOfDays ?? -7);
        var playerKills = await _context.PlayerKills
            .Join(_context.User,
                  pk => pk.Xuid,
                  u => u.Xuid,
                  (pk, u) => new { pk, PlayerName = u.Name })

            .Where(pku => pku.pk.KillTime > sevenDaysAgo)
            .GroupBy(pku => new { pku.pk.Xuid, pku.PlayerName, pku.pk.EntityType })
            .OrderByDescending(g => g.Count())
            .Select(g => new PlayerKillArticleDto
            {
                Xuid = g.Key.Xuid,
                PlayerName = g.Key.PlayerName ?? "Unknown",
                EntityType = g.Key.EntityType,
                KillTime = g.Max(pku => pku.pk.KillTime),
                KillCount = g.Count()
            })
            .Take(take ?? 2)
            .ToListAsync(ct);

        return playerKills;
    }

    public async Task<List<RealmEvent>> getRealmEventDetailsAsync(int? lastNumOfDays, int? take, CancellationToken ct)
    {
        var currentDate = DateTime.Now;
        var sevenDaysAgo = currentDate.AddDays(-lastNumOfDays ?? -7);
        var realmEvents = await _context.RealmEvent
            .Include(re => re.User)
            .Where(re => re.EventTime > sevenDaysAgo)
            .OrderByDescending(re => re.EventTime)
            .Take(take ?? 2)
            .ToListAsync(ct);
        return realmEvents;
    }

    public async Task GeneratePaper(int maxNumberOfArticles, CancellationToken ct)
    {
        var playerDeaths = await getPlayerDeathDetailsAsync(lastNumOfDays: 7, take: 2, ct);
        var realmEvents = await getRealmEventDetailsAsync(lastNumOfDays: 7, take: 2, ct);
        var playerKills = await getPlayerKillDetailsAsync(lastNumOfDays: 7, take: 4, ct);

        // Create the paper first
        var paper = new Paper
        {
            PublishDate = DateTime.Now,
            Articles = new List<Article>()
        };
        _context.Paper.Add(paper);
        await _context.SaveChangesAsync(ct);

        // Generate articles with reference to the paper ID only
        var playerDeathArticles = GeneratePlayerDeathsArticles(playerDeaths, paper.Id);
        var realmEventArticles = GenerateRealmEventArticles(realmEvents, paper.Id);
        var playerKillArticles = GeneratePlayerKillsArticles(playerKills, paper.Id);

        var playerDeathArticlesCount = playerDeathArticles.Count();
        var realmEventArticlesCount = realmEventArticles.Count();

        var paperArticles = new List<Article>();
        paperArticles.AddRange(playerDeathArticles);
        paperArticles.AddRange(realmEventArticles);
        paperArticles.AddRange(playerKillArticles.Take(maxNumberOfArticles - playerDeathArticlesCount - realmEventArticlesCount));

        //// Save each article individually to avoid issues with foreign key references
        foreach (var article in paperArticles)
        {
           _context.Article.Add(article);
           //await _context.SaveChangesAsync(ct);  // Save each article immediately to get its ID
        }
        await _context.SaveChangesAsync(ct);  // Final save to commit all articles
    }

    private IList<Article> GenerateRealmEventArticles(List<RealmEvent> realmEvents, int paperId)
    {
        var articles = new List<Article>();
        foreach (var realmEvent in realmEvents)
        {
            string playerName = realmEvent.User?.Name ?? "name not disclosed";
            articles.Add(new Article
            {
                PaperId = paperId,
                PlayerName = playerName,
                PlayerXuid = realmEvent.Xuid,
                RealmEventId = realmEvent.Id,
                Title = $"{playerName} {realmEvent.EventType}",
                Subtitle = $"{realmEvent.EventType} at {realmEvent.EventTime}",
                Content = $"Player {playerName} performed a realm event: {realmEvent.EventType} at {realmEvent.EventTime}.",
                PublishedDate = DateTime.Now,
                Tags = new List<string> { "RealmEvent", realmEvent.EventType },
            });
        }
        return articles;
    }

    private IList<Article> GeneratePlayerDeathsArticles(List<PlayerDeaths> playerDeaths, int paperId)
    {
        var articles = new List<Article>();

        foreach (var playerDeath in playerDeaths)
        {
            string playerName = playerDeath.Player?.Name ?? "name not disclosed";

            articles.Add(new Article
            {
                PaperId = paperId,
                PlayerDeathId = playerDeath.Id,
                PlayerXuid = playerDeath.Xuid,
                Title = $"Player Death: {playerName}",
                Subtitle = $"{playerDeath.Cause} at {playerDeath.DeathTime}",
                Content = $"Player {(playerDeath.Player?.Name ?? "Unknown")} died due to {playerDeath.Cause} at position ({playerDeath.PositionX}, {playerDeath.PositionY}, {playerDeath.PositionZ}) in dimension {playerDeath.Dimension}. They had spawned at ({playerDeath.SpawnPositionX}, {playerDeath.SpawnPositionY}, {playerDeath.SpawnPositionZ}).",
                PublishedDate = DateTime.Now,
                Tags = new List<string> { "PlayerDeath", playerDeath.Cause ?? "Unknown" },
                PlayerName = playerName,
                GameDay = playerDeath.GameDay,
                GameTime = playerDeath.GameTime ?? 0,
                PlayerSpawnX = playerDeath.SpawnPositionX ?? 0,
                PlayerSpawnY = playerDeath.SpawnPositionY ?? 0,
                PlayerSpawnZ = playerDeath.SpawnPositionZ ?? 0,
            });
        }
        return articles;
    }

    private IList<Article> GeneratePlayerKillsArticles(List<PlayerKillArticleDto> playerKills, int paperId)
    {
        var articles = new List<Article>();

        foreach (var playerKill in playerKills)
        {
            string playerName = playerKill.PlayerName ?? "Unknown";
            articles.Add(new Article
            {
                PaperId = paperId,
                PlayerKillsId = null, // No direct link to PlayerKills entity
                PlayerXuid = playerKill.Xuid,
                Title = $"Top Killer: {playerName}",
                Subtitle = $"{playerName} killed {playerKill.KillCount} {playerKill.EntityType}(s)",
                Content = $"Player {playerName} has killed {playerKill.KillCount} {playerKill.EntityType}(s), with the most recent kill at {playerKill.KillTime}.",
                PublishedDate = DateTime.Now,
                Tags = new List<string> { "PlayerKill", playerKill.EntityType },
                PlayerName = playerName,
            });
        }

        return articles;
    }
}

public class PlayerKillArticleDto
{
    public required string Xuid { get; set; }
    public required string PlayerName { get; set; }
    public required string EntityType { get; set; }
    public DateTime KillTime { get; set; }
    public int KillCount { get; set; }
}
