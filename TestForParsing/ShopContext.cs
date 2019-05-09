using Microsoft.EntityFrameworkCore;

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
