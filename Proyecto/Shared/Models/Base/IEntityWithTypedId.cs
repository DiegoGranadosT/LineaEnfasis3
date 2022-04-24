namespace Proyecto.Shared.Models.Base
{
    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}
