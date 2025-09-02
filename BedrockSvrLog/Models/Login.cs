using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BedrockSvrLog.Model
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Xuid")]
        public string Xuid { get; set; } // Foreign key to User table
        public DateTime LoginTime { get; set; }
        public DateTime? SpawnTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public TimeSpan? GameplayeDuration { get; set; }

        // Navigation property
        [ForeignKey("Xuid")]
        public User User { get; set; }

        public Login()
        {
        }
    }
}
