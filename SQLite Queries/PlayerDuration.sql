SELECT u.name, 
time(sum( strftime('%s', Duration) ), 'unixepoch') Duration,
time(sum( strftime('%s', GameplayeDuration) ), 'unixepoch') GameplayDuration
FROM Login l
JOIN User u on l.Xuid = u.Xuid
GROUP BY u.name
ORDER BY Duration DESC;