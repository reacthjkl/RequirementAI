namespace RequirementAI.Business.Interfaces;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken ct);
}