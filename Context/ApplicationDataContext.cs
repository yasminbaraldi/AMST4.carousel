using AMST4.CAROUSEL.Models;
using Microsoft.EntityFrameworkCore;

namespace AMST4.CAROUSEL.Context
{
    public class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> option) : base(option) { }
        public DbSet<Category> Category { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Product> Products { get; set; } = default;
    }
}
