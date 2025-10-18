using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserInfoPanel
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; }


    [ForeignKey("BloodPanel")]
    public int BloodPanelId { get; set; }

    public BloodPanel BloodPanel { get; set; }

    [ForeignKey("LipidPanel")]
    public int LipidPanelId { get; set; }

    public LipidPanel LipidPanel { get; set; }

    [ForeignKey("MetabolicPanel")]
    public int MetabolicPanelId { get; set; }

    public MetabolicPanel MetabolicPanel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}