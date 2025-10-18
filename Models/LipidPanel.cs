using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LipidPanel
{
    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "Total Cholesterol must be a positive value.")]
    public double TotalCholesterol { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "LDL must be a positive value.")]
    public double LDL { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "HDL must be a positive value.")]
    public double HDL { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "Triglycerides must be a positive value.")]
    public double Triglycerides { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}