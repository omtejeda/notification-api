using System;
namespace NotificationService.Common.Entities
{
    public abstract class BaseEntity
    {
        public virtual string Id { get; protected set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual bool? Deleted { get; set; } = false;
    }
}