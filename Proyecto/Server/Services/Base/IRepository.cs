using Proyecto.Shared.Models.Base;

namespace Proyecto.Server.Services.Base
{ 
    /// <summary>
    /// Implementación del Patron de Diseño Repository y del Patron UnitOfWork
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IRepository<T, TId> : IRepositoryWithTypedId<T, TId>
        where T : class, IEntityWithTypedId<TId>
    {
    }
}
