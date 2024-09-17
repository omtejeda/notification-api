namespace NotificationService.Domain.Entities;

public class Catalog : BaseEntity
{
    public string CatalogId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public ICollection<Element> Elements { get; set; } = [];
    public int? ElementsCount => Elements?.Count;
}

public class Element
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public ICollection<Label> Labels { get; set; } = [];
}

public class Label
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}