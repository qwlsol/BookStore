using BookStore.Model;
using BookStore.Model.AuthApp;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            //Database.Migrate();
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthUser> AuthUsers { get; set; }
    }
}
