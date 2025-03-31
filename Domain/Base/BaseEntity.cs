using System.ComponentModel.DataAnnotations;


namespace Domain.Base;

public class BaseEntity<TId> : IEntity<TId>, ISoftDelete
{
    [Key]
    public TId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
    public bool? IsDeleted { get; set; } = false;

    public bool? Status { get; set; } = false;

    protected BaseEntity()
    {

        CreatedOn = DateTime.UtcNow;
       // LastModifiedOn = DateTime.UtcNow;
    }
}
