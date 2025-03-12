
namespace Domain.Base;

public class GuidBaseEntity:BaseEntity<string>
{
    public GuidBaseEntity() : base()
    {
        Id = Guid.NewGuid().ToString().ToUpper();
    }
}
