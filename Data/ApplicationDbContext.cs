using Microsoft.EntityFrameworkCore;
using BookStore.Model;

namespace BookStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
    }
}
