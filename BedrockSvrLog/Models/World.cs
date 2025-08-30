using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BedrockSvrLog.Data
{
    public class World
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CurrentTime { get; set; } = string.Empty;
        public int CurrentDay { get; set; }
        public string Seed { get; set; } = string.Empty;

        public int? SpawnX { get; set; }
        public int? SpawnY { get; set; }
        public int? SpawnZ { get; set; }
    }
}