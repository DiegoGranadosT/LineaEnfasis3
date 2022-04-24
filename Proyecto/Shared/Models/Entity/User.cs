using Proyecto.Shared.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Shared.Models.Entity
{
    public class User : EntityBase<int>
    {
        public string UserName { get; set; }
        public byte[] PasswordHast { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
