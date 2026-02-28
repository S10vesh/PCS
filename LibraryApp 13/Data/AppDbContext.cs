using Microsoft.EntityFrameworkCore;
using LibraryApp13.Models;

namespace LibraryApp13.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=library.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.Id);
            modelBuilder.Entity<Author>().HasKey(a => a.Id);
            modelBuilder.Entity<Genre>().HasKey(g => g.Id);

            modelBuilder.Entity<Book>()
                .Property(b => b.Title).IsRequired().HasMaxLength(200);

            modelBuilder.Entity<Book>()
                .Property(b => b.ISBN).HasMaxLength(20);

            modelBuilder.Entity<Author>()
                .Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Author>()
                .Property(a => a.LastName).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Genre>()
                .Property(g => g.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}