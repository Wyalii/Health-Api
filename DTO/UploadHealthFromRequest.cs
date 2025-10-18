public class UploadHealthFormRequest
{
    public int UserId { get; set; }
    public double? WBC { get; set; }
    public double? RBC { get; set; }
    public double? Hemoglobin { get; set; }
    public double? Hematocrit { get; set; }
    public double? Platelets { get; set; }


    public double? Glucose { get; set; }
    public double? Creatinine { get; set; }
    public double? eGFR { get; set; }
    public double? Sodium { get; set; }
    public double? Potassium { get; set; }
    public double? Calcium { get; set; }
    public double? ALT { get; set; }
    public double? Albumin { get; set; }


    public double? TotalCholesterol { get; set; }
    public double? LDL { get; set; }
    public double? HDL { get; set; }
    public double? Triglycerides { get; set; }
}