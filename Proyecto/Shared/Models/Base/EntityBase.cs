using System;

namespace Proyecto.Shared.Models.Base
{
    public class EntityBase<TId> : EntityWithTypedId<TId>
    {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public EntityBase()
        {
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}
