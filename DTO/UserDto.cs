public class UserDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public BloodPanelDto? UserBloodPanel { get; set; }
    public LipidPanelDto? UserLipidPanel { get; set; }
    public MetabolicPanelDto? MetabolicPanel { get; set; }
}