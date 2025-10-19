using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AIController : ControllerBase
{
    private readonly GoogleAIService _googleAIService;
    private readonly UsersRepository _usersRepository;

    public AIController(GoogleAIService googleAIService, UsersRepository usersRepository)
    {
        _googleAIService = googleAIService;
        _usersRepository = usersRepository;
    }

    [HttpGet("AnalyzeUserLabData")]
    public async Task<IActionResult> AnalyzeUserLabData()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        int id = int.Parse(userId);
        var userInfo = await _usersRepository.GetUserInfoAsync(id);
        if (userInfo == null) return NotFound();

        var analysisRequest = new HealthAnalysisRequest
        {
            WBC = userInfo.UserBloodPanel?.WBC ?? 0,
            RBC = userInfo.UserBloodPanel?.RBC ?? 0,
            Hemoglobin = userInfo.UserBloodPanel?.Hemoglobin ?? 0,
            Hematocrit = userInfo.UserBloodPanel?.Hematocrit ?? 0,
            Platelets = userInfo.UserBloodPanel?.Platelets ?? 0,
            TotalCholesterol = userInfo.UserLipidPanel?.TotalCholesterol ?? 0,
            LDL = userInfo.UserLipidPanel?.LDL ?? 0,
            HDL = userInfo.UserLipidPanel?.HDL ?? 0,
            Triglycerides = userInfo.UserLipidPanel?.Triglycerides ?? 0,
            Albumin = userInfo.MetabolicPanel?.Albumin ?? 0,
            ALT = userInfo.MetabolicPanel?.ALT ?? 0,
            Calcium = userInfo.MetabolicPanel?.Calcium ?? 0,
            Creatinine = userInfo.MetabolicPanel?.Creatinine ?? 0,
            eGFR = userInfo.MetabolicPanel?.eGFR ?? 0,
            Glucose = userInfo.MetabolicPanel?.GlucoseFasting ?? 0,
            Potassium = userInfo.MetabolicPanel?.Potassium ?? 0,
            Sodium = userInfo.MetabolicPanel?.Sodium ?? 0
        };

        var aiResult = await _googleAIService.AnalyzeHealthAsync(analysisRequest);

        return Ok(new
        {
            Success = true,
            AIAnalysis = aiResult
        });
    }


}