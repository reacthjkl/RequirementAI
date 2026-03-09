using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RequirementAI.Business.Interfaces;

namespace RequirementAI.Business.Services;

public class EmailService(IConfiguration configuration, ILogger<IEmailService> logger, HttpClient http) : IEmailService
{
    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct)
    {
        var payload = new
        {
            sender = new
            {
                name = configuration["Email:FromName"],
                email = configuration["Email:From"]
            },
            to = new[] { new { email = to } },
            subject,
            htmlContent = htmlBody
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
        request.Headers.Add("api-key", configuration["Email:ApiKey"]);
        request.Content = JsonContent.Create(payload);

        var response = await http.SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Brevo API failed: {Status} {Body}", response.StatusCode, body);
            throw new Exception($"Failed to send email: {response.StatusCode}");
        }

        logger.LogInformation("Email sent to {Email}", to);
    }
}