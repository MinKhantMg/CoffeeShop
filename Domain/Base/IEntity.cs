
namespace Domain.Base;

public interface IEntity<TId>
{
    TId Id { get; set; }
}
