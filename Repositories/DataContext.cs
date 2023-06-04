using eFilingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace eFilingAPI.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<Post> Post { get; set; }
    }
}
