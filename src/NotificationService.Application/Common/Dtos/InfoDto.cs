namespace NotificationService.Application.Common.Dtos;

public class InfoDto
{
    public bool IsProduction { get; set; }
    public string? Environment { get; set; }
    public DateTime SystemDate { get; set; }
    public int? Gmt { get; set; }
}