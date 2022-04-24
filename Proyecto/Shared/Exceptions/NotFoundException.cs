using System;

namespace Proyecto.Shared.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) no fué encontrado")
        {
        }
    }
}
