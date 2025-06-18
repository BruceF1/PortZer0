using Microsoft.EntityFrameworkCore;
using PortZer0.Models;

namespace PortZer0.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BlogPost> BlogPost { get; set; }
    }
}
