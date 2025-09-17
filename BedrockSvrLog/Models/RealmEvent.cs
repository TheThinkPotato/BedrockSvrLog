using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BedrockSvrLog.Model
{
    public class RealmEvent
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Xuid")]
        public string Xuid { get; set; } // Foreign key to User table
        public virtual User User { get; set; }
        
        public string EventType { get; set; } = string.Empty;
        public DateTime EventTime { get; set; }

        public RealmEvent()
        {
        }
    }
}
