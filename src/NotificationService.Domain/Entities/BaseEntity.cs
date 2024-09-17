namespace NotificationService.Domain.Entities
{
    public abstract class BaseEntity
    {
        public virtual string Id { get; protected set; } = string.Empty;
        public virtual DateTime? CreatedOn { get; set; }
        public virtual string CreatedBy { get; set; } = string.Empty;
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual string? ModifiedBy { get; set; }
        public virtual bool? Deleted { get; set; } = false;
    }
}