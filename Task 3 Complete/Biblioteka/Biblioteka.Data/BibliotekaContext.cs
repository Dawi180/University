
using Biblioteka.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.Data
{
    public class BibliotekaContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=Biblioteka.db");
            }
        }

        public DbSet<Models.Book> Books { get; set; }
        public DbSet<Models.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.User)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
