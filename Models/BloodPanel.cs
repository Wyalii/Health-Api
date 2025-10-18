using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BloodPanel
{

    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "WBC must be a positive value.")]
    public double WBC { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "RBC must be a positive value.")]
    public double RBC { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Hemoglobin must be a positive value.")]
    public double Hemoglobin { get; set; }

    [Range(0, 100, ErrorMessage = "Hematocrit must be between 0 and 100.")]
    public double Hematocrit { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Platelets must be a positive value.")]
    public double Platelets { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}