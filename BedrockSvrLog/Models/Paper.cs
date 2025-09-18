using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BedrockSvrLog.Models;

public class Paper
{
    [Key]
    public int Id { get; set; }
    public DateTime PublishDate { get; set; }
    public List<Article> Articles { get; set; } = new List<Article>();
}
