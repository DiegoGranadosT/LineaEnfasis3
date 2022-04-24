using Microsoft.EntityFrameworkCore;
using Proyecto.Shared.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProyectoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ProyectoDbContext()
        {

        }

        public ProyectoDbContext(DbContextOptions<ProyectoDbContext> options) : base(options)
        {

        }
    }
}
