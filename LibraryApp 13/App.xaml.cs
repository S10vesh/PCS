using System;
using System.Linq;
using System.Windows;
using LibraryApp13.Data;
using LibraryApp13.Models;

namespace LibraryApp13
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new AppDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                SeedData(context);
            }
        }

        private void SeedData(AppDbContext context)
        {
            if (context.Authors.Any() || context.Genres.Any() || context.Books.Any())
                return;

            var authors = new[]
            {
                new Author { FirstName = "Лев", LastName = "Толстой", BirthDate = new DateTime(1828, 9, 9), Country = "Россия" },
                new Author { FirstName = "Федор", LastName = "Достоевский", BirthDate = new DateTime(1821, 11, 11), Country = "Россия" },
                new Author { FirstName = "Антон", LastName = "Чехов", BirthDate = new DateTime(1860, 1, 29), Country = "Россия" },
                new Author { FirstName = "Александр", LastName = "Пушкин", BirthDate = new DateTime(1799, 6, 6), Country = "Россия" }
            };
            context.Authors.AddRange(authors);
            context.SaveChanges();

            var genres = new[]
            {
                new Genre { Name = "Роман", Description = "Крупное эпическое произведение" },
                new Genre { Name = "Рассказ", Description = "Малое эпическое произведение" },
                new Genre { Name = "Поэзия", Description = "Стихотворные произведения" },
                new Genre { Name = "Драма", Description = "Произведения для театра" }
            };
            context.Genres.AddRange(genres);
            context.SaveChanges();

            var books = new[]
            {
                new Book { Title = "Война и мир", PublishYear = 1869, ISBN = "978-5-17-123456-7", QuantityInStock = 10, AuthorId = authors[0].Id, GenreId = genres[0].Id },
                new Book { Title = "Анна Каренина", PublishYear = 1877, ISBN = "978-5-17-123457-4", QuantityInStock = 8, AuthorId = authors[0].Id, GenreId = genres[0].Id },
                new Book { Title = "Преступление и наказание", PublishYear = 1866, ISBN = "978-5-17-123458-1", QuantityInStock = 5, AuthorId = authors[1].Id, GenreId = genres[0].Id },
                new Book { Title = "Идиот", PublishYear = 1869, ISBN = "978-5-17-123459-8", QuantityInStock = 3, AuthorId = authors[1].Id, GenreId = genres[0].Id },
                new Book { Title = "Вишневый сад", PublishYear = 1904, ISBN = "978-5-17-123460-4", QuantityInStock = 7, AuthorId = authors[2].Id, GenreId = genres[3].Id },
                new Book { Title = "Евгений Онегин", PublishYear = 1833, ISBN = "978-5-17-123461-1", QuantityInStock = 12, AuthorId = authors[3].Id, GenreId = genres[2].Id }
            };
            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}