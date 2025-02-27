namespace NotificationService.Application.Contracts.DTOs.Responses;

public class CatalogDto
{
    public string? CatalogId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public ICollection<ElementDto> Elements { get; set; } = [];
    public int? ElementsCount => Elements?.Count;
}

public class ElementDto
{
    public string? Key { get; set; }
    public string? Value { get; set; }
    public ICollection<LabelDto> Labels { get; set; } = [];
}

public class LabelDto
{
    public string? Key { get; set; }
    public string? Value { get; set; }
}