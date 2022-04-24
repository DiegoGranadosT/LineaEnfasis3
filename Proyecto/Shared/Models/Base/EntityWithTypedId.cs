namespace Proyecto.Shared.Models.Base
{
    public abstract class EntityWithTypedId<TId> : IEntityWithTypedId<TId>
    {
        public TId Id { get; set; }
    }
}
