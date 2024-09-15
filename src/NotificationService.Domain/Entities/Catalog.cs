namespace NotificationService.Domain.Entities;

public class Catalog : BaseEntity
{
    public string CatalogId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool? IsActive { get; set; }
    public ICollection<Element> Elements { get; set; }
    public int? ElementsCount => Elements?.Count;
}

public class Element
{
    public string Key { get; set; }
    public string Value { get; set; }
    public ICollection<Label> Labels { get; set; }
}

public class Label
{
    public string Key { get; set; }
    public string Value { get; set; }
}