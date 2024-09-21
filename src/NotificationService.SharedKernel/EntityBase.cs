namespace NotificationService.SharedKernel;

public abstract class EntityBase
{
    public virtual string Id { get; protected set; } = string.Empty;
    public virtual DateTime? CreatedOn { get; set; }
    public virtual string CreatedBy { get; set; } = string.Empty;
    public virtual DateTime? ModifiedOn { get; set; }
    public virtual string? ModifiedBy { get; set; }
    public virtual bool? Deleted { get; set; } = false;
}