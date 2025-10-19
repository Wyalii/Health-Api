using Microsoft.EntityFrameworkCore;

public class PanelsRepository
{
    private readonly AppDbContext _context;
    public PanelsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> CreateUserInfo(UploadHealthFormRequest uploadHealthFormRequest)
    {
        if (uploadHealthFormRequest == null)
            return new { Success = false, Message = "Invalid request data." };

        var userId = uploadHealthFormRequest.UserId;


        var userExists = await _context.Users.FindAsync(userId);
        if (userExists == null)
            return new { Success = false, Message = "User not found." };

        string overallStatus = "Optimal";

        void EvaluateStatus(double? value, double min, double max)
        {
            if (!value.HasValue)
                return;

            if (value < min * 0.8 || value > max * 1.2)
                overallStatus = "Critical";
            else if (value < min || value > max)
            {
                if (overallStatus != "Critical")
                    overallStatus = "Suboptimal";
            }
        }


        var bloodPanel = await _context.BloodPanel.FirstOrDefaultAsync(b => b.UserId == userId);
        if (uploadHealthFormRequest.WBC.HasValue || uploadHealthFormRequest.RBC.HasValue ||
            uploadHealthFormRequest.Hemoglobin.HasValue || uploadHealthFormRequest.Hematocrit.HasValue ||
            uploadHealthFormRequest.Platelets.HasValue)
        {
            if (bloodPanel == null)
            {
                bloodPanel = new BloodPanel { UserId = userId };
                _context.BloodPanel.Add(bloodPanel);
            }

            bloodPanel.WBC = uploadHealthFormRequest.WBC ?? bloodPanel.WBC;
            bloodPanel.RBC = uploadHealthFormRequest.RBC ?? bloodPanel.RBC;
            bloodPanel.Hemoglobin = uploadHealthFormRequest.Hemoglobin ?? bloodPanel.Hemoglobin;
            bloodPanel.Hematocrit = uploadHealthFormRequest.Hematocrit ?? bloodPanel.Hematocrit;
            bloodPanel.Platelets = uploadHealthFormRequest.Platelets ?? bloodPanel.Platelets;

            EvaluateStatus(uploadHealthFormRequest.WBC, 4.0, 8.0);
            EvaluateStatus(uploadHealthFormRequest.RBC, 4.2, 5.5);
            EvaluateStatus(uploadHealthFormRequest.Hemoglobin, 13.5, 16.5);
            EvaluateStatus(uploadHealthFormRequest.Hematocrit, 40, 50);
            EvaluateStatus(uploadHealthFormRequest.Platelets, 150, 350);
            await _context.SaveChangesAsync();
        }


        var metabolicPanel = await _context.MetabolicPanel.FirstOrDefaultAsync(m => m.UserId == userId);
        if (uploadHealthFormRequest.Glucose.HasValue || uploadHealthFormRequest.Creatinine.HasValue ||
            uploadHealthFormRequest.eGFR.HasValue || uploadHealthFormRequest.Sodium.HasValue ||
            uploadHealthFormRequest.Potassium.HasValue || uploadHealthFormRequest.Calcium.HasValue ||
            uploadHealthFormRequest.ALT.HasValue || uploadHealthFormRequest.Albumin.HasValue)
        {
            if (metabolicPanel == null)
            {
                metabolicPanel = new MetabolicPanel { UserId = userId };
                _context.MetabolicPanel.Add(metabolicPanel);
            }

            metabolicPanel.GlucoseFasting = uploadHealthFormRequest.Glucose ?? metabolicPanel.GlucoseFasting;
            metabolicPanel.Creatinine = uploadHealthFormRequest.Creatinine ?? metabolicPanel.Creatinine;
            metabolicPanel.eGFR = uploadHealthFormRequest.eGFR ?? metabolicPanel.eGFR;
            metabolicPanel.Sodium = uploadHealthFormRequest.Sodium ?? metabolicPanel.Sodium;
            metabolicPanel.Potassium = uploadHealthFormRequest.Potassium ?? metabolicPanel.Potassium;
            metabolicPanel.Calcium = uploadHealthFormRequest.Calcium ?? metabolicPanel.Calcium;
            metabolicPanel.ALT = uploadHealthFormRequest.ALT ?? metabolicPanel.ALT;
            metabolicPanel.Albumin = uploadHealthFormRequest.Albumin ?? metabolicPanel.Albumin;

            EvaluateStatus(uploadHealthFormRequest.Glucose, 75, 90);
            EvaluateStatus(uploadHealthFormRequest.Creatinine, 0.7, 1.1);
            if (uploadHealthFormRequest.eGFR.HasValue && uploadHealthFormRequest.eGFR < 90)
                overallStatus = "Suboptimal";
            EvaluateStatus(uploadHealthFormRequest.Sodium, 137, 142);
            EvaluateStatus(uploadHealthFormRequest.Potassium, 4.0, 4.8);
            EvaluateStatus(uploadHealthFormRequest.Calcium, 9.2, 10.0);
            if (uploadHealthFormRequest.ALT.HasValue && uploadHealthFormRequest.ALT >= 25)
                overallStatus = "Suboptimal";
            EvaluateStatus(uploadHealthFormRequest.Albumin, 4.2, 5.0);
            await _context.SaveChangesAsync();
        }


        var lipidPanel = await _context.LipidPanel.FirstOrDefaultAsync(l => l.UserId == userId);
        if (uploadHealthFormRequest.TotalCholesterol.HasValue || uploadHealthFormRequest.LDL.HasValue ||
            uploadHealthFormRequest.HDL.HasValue || uploadHealthFormRequest.Triglycerides.HasValue)
        {
            if (lipidPanel == null)
            {
                lipidPanel = new LipidPanel { UserId = userId };
                _context.LipidPanel.Add(lipidPanel);
            }

            lipidPanel.TotalCholesterol = uploadHealthFormRequest.TotalCholesterol ?? lipidPanel.TotalCholesterol;
            lipidPanel.LDL = uploadHealthFormRequest.LDL ?? lipidPanel.LDL;
            lipidPanel.HDL = uploadHealthFormRequest.HDL ?? lipidPanel.HDL;
            lipidPanel.Triglycerides = uploadHealthFormRequest.Triglycerides ?? lipidPanel.Triglycerides;

            EvaluateStatus(uploadHealthFormRequest.TotalCholesterol, 150, 180);
            if (uploadHealthFormRequest.LDL.HasValue && uploadHealthFormRequest.LDL >= 100)
                overallStatus = "Suboptimal";
            if (uploadHealthFormRequest.HDL.HasValue && uploadHealthFormRequest.HDL <= 60)
                overallStatus = "Suboptimal";
            if (uploadHealthFormRequest.Triglycerides.HasValue && uploadHealthFormRequest.Triglycerides >= 90)
                overallStatus = "Suboptimal";
            await _context.SaveChangesAsync();
        }
        var userInfoPanel = await _context.UserInfoPanel.FirstOrDefaultAsync(u => u.UserId == userId);
        if (userInfoPanel == null)
        {
            userInfoPanel = new UserInfoPanel
            {
                UserId = userId,
                BloodPanelId = bloodPanel?.Id ?? 0,
                LipidPanelId = lipidPanel?.Id ?? 0,
                MetabolicPanelId = metabolicPanel?.Id ?? 0,
                CreatedAt = DateTime.UtcNow
            };
            _context.UserInfoPanel.Add(userInfoPanel);
        }
        else
        {
            userInfoPanel.BloodPanelId = bloodPanel?.Id ?? userInfoPanel.BloodPanelId;
            userInfoPanel.LipidPanelId = lipidPanel?.Id ?? userInfoPanel.LipidPanelId;
            userInfoPanel.MetabolicPanelId = metabolicPanel?.Id ?? userInfoPanel.MetabolicPanelId;
            userInfoPanel.CreatedAt = DateTime.UtcNow;
        }



        await _context.SaveChangesAsync();

        return new
        {
            Success = true,
            Message = "Health data saved successfully.",
            OverallStatus = overallStatus
        };
    }
}