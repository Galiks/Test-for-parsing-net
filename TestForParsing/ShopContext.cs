using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForParsing
{
    class ShopContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;user id=root;persistsecurityinfo=True;database=forvisualstudio;password=admin");
        }
    }
}
