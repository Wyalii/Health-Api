using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;
using System;

public class GoogleAIService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey;

    public GoogleAIService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = Environment.GetEnvironmentVariable("GOOGLE_AI_API_KEY");
        if (string.IsNullOrEmpty(_apiKey))
            throw new InvalidOperationException("GOOGLE_AI_API_KEY not set in environment.");
    }

    public async Task<string> AnalyzeHealthAsync(HealthAnalysisRequest request)
    {
        var client = _httpClientFactory.CreateClient();

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

        var promptBuilder = new StringBuilder();
        promptBuilder.AppendLine("Analyze this patient's lab results and provide a clear summary and actionable advice.\n");

        // 🩸 Blood Panel
        promptBuilder.AppendLine("Blood Panel:");
        if (request.WBC != 0) promptBuilder.AppendLine($"- WBC: {request.WBC}");
        if (request.RBC != 0) promptBuilder.AppendLine($"- RBC: {request.RBC}");
        if (request.Hemoglobin != 0) promptBuilder.AppendLine($"- Hemoglobin: {request.Hemoglobin}");
        if (request.Hematocrit != 0) promptBuilder.AppendLine($"- Hematocrit: {request.Hematocrit}");
        if (request.Platelets != 0) promptBuilder.AppendLine($"- Platelets: {request.Platelets}");

        promptBuilder.AppendLine("\nLipid Panel:");
        if (request.TotalCholesterol != 0) promptBuilder.AppendLine($"- Total Cholesterol: {request.TotalCholesterol}");
        if (request.LDL != 0) promptBuilder.AppendLine($"- LDL: {request.LDL}");
        if (request.HDL != 0) promptBuilder.AppendLine($"- HDL: {request.HDL}");
        if (request.Triglycerides != 0) promptBuilder.AppendLine($"- Triglycerides: {request.Triglycerides}");

        string promptText = promptBuilder.ToString();

        // ✅ 2.5 structure with thinking support
        var payload = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = promptText }
                    }
                }
            },
            generationConfig = new
            {
                thinkingConfig = new
                {
                    thinkingBudget = 1024,
                    includeThoughts = true
                }
            }
        };

        var response = await client.PostAsJsonAsync(url, payload);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Gemini API error: {json}");

        using var doc = JsonDocument.Parse(json);

        string thoughts = "";
        string answer = "";

        var parts = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts");

        foreach (var part in parts.EnumerateArray())
        {
            if (part.TryGetProperty("thought", out var thoughtFlag) && thoughtFlag.GetBoolean())
                thoughts += part.GetProperty("text").GetString() + "\n";
            else if (part.TryGetProperty("text", out var textProp))
                answer += textProp.GetString() + "\n";
        }

        return $"🧠 Thoughts:\n{thoughts}\n💡 Answer:\n{answer}";
    }
}
