using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PanelsController : ControllerBase
{
    private readonly PanelsRepository _panelsRepository;
    public PanelsController(PanelsRepository panelsRepository)
    {
        _panelsRepository = panelsRepository;
    }

    [HttpPost("CreateLabData")]
    public async Task<IActionResult> CreateLabData(UploadHealthFormRequest uploadHealthFormRequest)
    {
        if (uploadHealthFormRequest == null)
            return BadRequest(new { Success = false, Message = "Request body cannot be empty." });

        try
        {
            var result = await _panelsRepository.CreateUserInfo(uploadHealthFormRequest);
            dynamic response = result;
            if (!response.Success)
                return BadRequest(result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = "An error occurred while processing the request.",
                Error = ex.Message
            });
        }
    }
}