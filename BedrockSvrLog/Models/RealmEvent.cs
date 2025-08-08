using BedrockSvrLog.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BedrockSvrLog.Data
{
    public class RealmEvent
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Xuid")]
        public string Xuid { get; set; } = string.Empty; // Foreign key to User table
        public string EventType { get; set; } = string.Empty;
        public DateTime EventTime { get; set; }

        // Navigation property
        [ForeignKey("Xuid")]
        public User User { get; set; }

        public RealmEvent()
        {
        }
    }
}
