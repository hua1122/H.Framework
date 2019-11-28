using Microsoft.EntityFrameworkCore;
using Test.NUnit.Model;

namespace Test.NUnit
{
    public class InMemoryContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("test");
        }
    }
}
