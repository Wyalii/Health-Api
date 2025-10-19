
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersRepository _usersRepository;
    private readonly TokenService _tokenService;
    public UsersController(UsersRepository userRepository, TokenService tokenService)
    {
        _usersRepository = userRepository;
        _tokenService = tokenService;
    }

    [Authorize]
    [HttpPatch("UpdateProfile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto updateUserDto)
    {
        try
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found.");
            }

            int user_id = int.Parse(userIdClaim.Value);
            User UpdatedUser = await _usersRepository.UpdateUserAsync(user_id, updateUserDto.Username, updateUserDto.Email, updateUserDto.Password);
            string updatedUserId = UpdatedUser.Id.ToString();
            string newToken = _tokenService.GenerateToken(updatedUserId, UpdatedUser.Email);

            if (UpdatedUser == null)
            {
                return Unauthorized(new { message = "User not found." });
            }
            return Ok(new { message = $"Profile updated.", token = newToken, User = UpdatedUser });
        }
        catch (Exception ex)
        {

            return StatusCode(500, $"Internal server error: {ex.Message}");
        }

    }
    // [HttpPost("DecodeToken")]
    // public IActionResult DecodeToken(object jwtRegisteredClaimNames)
    // {
    //     try
    //     {
    //         var authHeader = Request.Headers["Authorization"].FirstOrDefault();
    //         if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
    //         {
    //             return Unauthorized("Missing or invalid Authorization header.");
    //         }
    //         var token = authHeader.Substring("Bearer ".Length).Trim();
    //         var claims = _tokenService.GetClaimsFromToken(token);
    //         if (claims == null)
    //         {
    //             return Unauthorized("Invalid or expired token.");
    //         }

    //         var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value
    //           ?? claims.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

    //         var email = claims.FindFirst(ClaimTypes.Email)?.Value
    //                   ?? claims.FindFirst(JwtRegisteredClaimNames.Email)?.Value;


    //         if (userId == null || email == null)
    //         {
    //             return Unauthorized("Required claims not found.");
    //         }

    //         return Ok(new
    //         {
    //             Id = userId,
    //             Email = email
    //         });
    //     }
    //     catch (Exception ex)
    //     {

    //         return StatusCode(500, $"Internal server error: {ex.Message}");
    //     }

    // }
    [HttpGet("GetUserInfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            int id = int.Parse(userId);
            var userInfo = await _usersRepository.GetUserInfoAsync(id);

            if (userInfo == null)
                return NotFound();
            var userDto = new UserDto
            {
                Username = userInfo.Username,
                Email = userInfo.Email,
                UserBloodPanel = userInfo.UserBloodPanel != null
                ? new BloodPanelDto
                {

                    Hemoglobin = userInfo.UserBloodPanel.Hemoglobin,
                    Wbc = userInfo.UserBloodPanel.WBC,
                    Platelets = userInfo.UserBloodPanel.Platelets,
                    RBC = userInfo.UserBloodPanel.RBC,
                    Hematocrit = userInfo.UserBloodPanel.Hematocrit,
                    CreatedAt = userInfo.UserBloodPanel.CreatedAt

                }
                : null,
                UserLipidPanel = userInfo.UserLipidPanel != null
                ? new LipidPanelDto
                {

                    TotalCholesterol = userInfo.UserLipidPanel.TotalCholesterol,
                    LDL = userInfo.UserLipidPanel.LDL,
                    HDL = userInfo.UserLipidPanel.HDL,
                    Triglycerides = userInfo.UserLipidPanel.Triglycerides,
                    CreatedAt = userInfo.UserLipidPanel.CreatedAt
                }
                : null,
                MetabolicPanel = userInfo.MetabolicPanel != null
                ? new MetabolicPanelDto
                {

                    Glucose = userInfo.MetabolicPanel.GlucoseFasting,
                    Calcium = userInfo.MetabolicPanel.Calcium,
                    Sodium = userInfo.MetabolicPanel.Sodium,
                    Albumin = userInfo.MetabolicPanel.Albumin,
                    ALT = userInfo.MetabolicPanel.ALT,
                    Potassium = userInfo.MetabolicPanel.Potassium,
                    eGFR = userInfo.MetabolicPanel.eGFR,
                    Creatinine = userInfo.MetabolicPanel.Creatinine,
                    CreatedAt = userInfo.MetabolicPanel.CreatedAt

                }
                : null
            };

            return Ok(userDto);
        }
        catch (Exception ex)
        {

            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}