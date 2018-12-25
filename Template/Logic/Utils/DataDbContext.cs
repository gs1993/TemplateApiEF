using Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace Logic.Utils
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
