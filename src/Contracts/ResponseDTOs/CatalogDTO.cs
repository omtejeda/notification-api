using System.Collections.Generic;
namespace NotificationService.Contracts.ResponseDtos
{
    public class CatalogDTO
    {
        public string CatalogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public ICollection<ElementDTO> Elements { get; set; }
        public int? ElementsCount => Elements?.Count;
    }

    public class ElementDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public ICollection<LabelDTO> Labels { get; set; }
    }

    public class LabelDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}