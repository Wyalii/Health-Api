using System.ComponentModel.DataAnnotations;

public class MetabolicPanel
{
    public int Id { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "Glucose (fasting) must be a positive value.")]
    public double GlucoseFasting { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "Creatinine must be a positive value.")]
    public double Creatinine { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "eGFR must be a positive value.")]
    public double eGFR { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "Sodium must be a positive value.")]
    public double Sodium { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "Potassium must be a positive value.")]
    public double Potassium { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "Calcium must be a positive value.")]
    public double Calcium { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "ALT must be a positive value.")]
    public double ALT { get; set; }


    [Range(0, double.MaxValue, ErrorMessage = "Albumin must be a positive value.")]
    public double Albumin { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}