using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Column("id")]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public BloodPanel UserBloodPanel { get; set; }
    public LipidPanel UserLipidPanel { get; set; }
    public MetabolicPanel MetabolicPanel { get; set; }
    public int? UserBloodPanelId { get; set; }
    public int? UserLipidPanelId { get; set; }
    public int? MetabolicPanelId { get; set; }
}