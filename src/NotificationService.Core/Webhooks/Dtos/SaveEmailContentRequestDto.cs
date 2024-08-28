namespace NotificationService.Core.Webhooks.Dtos;

public record SaveEmailContentRequestDto
{
    public string Email { get; set; }
    public string To { get; set; }
    public string From { get; set; }
    public string Subject { get; set; }
    public string Text { get; set; }
    public string Html { get; set; }
    public string Headers {get; set;}

}
