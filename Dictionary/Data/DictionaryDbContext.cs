using Dictionary.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Data
{
    public class DictionaryDbContext : DbContext
    {
        public DictionaryDbContext(DbContextOptions options) : base(options)
        {
        }

        protected DictionaryDbContext()
        {
        }

        public DbSet<Definition> Definitions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}