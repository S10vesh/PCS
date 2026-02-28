using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using LibraryApp13.Data;
using LibraryApp13.Models;

namespace LibraryApp13
{
    public partial class MainWindow : Window
    {
        private AppDbContext _context = new AppDbContext();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Загружаем авторов для фильтра
            var authors = _context.Authors.ToList();

            // Создаем список с пунктом "Все"
            var authorList = new System.Collections.Generic.List<object>();
            authorList.Add(new { Id = 0, LastName = "Все авторы" });
            foreach (var a in authors)
            {
                authorList.Add(new { a.Id, a.LastName });
            }

            AuthorFilterComboBox.ItemsSource = authorList;
            AuthorFilterComboBox.DisplayMemberPath = "LastName";
            AuthorFilterComboBox.SelectedValuePath = "Id";
            AuthorFilterComboBox.SelectedIndex = 0;

            // Загружаем жанры для фильтра
            var genres = _context.Genres.ToList();

            // Создаем список с пунктом "Все"
            var genreList = new System.Collections.Generic.List<object>();
            genreList.Add(new { Id = 0, Name = "Все жанры" });
            foreach (var g in genres)
            {
                genreList.Add(new { g.Id, g.Name });
            }

            GenreFilterComboBox.ItemsSource = genreList;
            GenreFilterComboBox.DisplayMemberPath = "Name";
            GenreFilterComboBox.SelectedValuePath = "Id";
            GenreFilterComboBox.SelectedIndex = 0;

            LoadBooks();
        }

        private void LoadBooks()
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();

            // Поиск по названию
            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                query = query.Where(b => b.Title.Contains(SearchTextBox.Text));
            }

            // Фильтр по автору
            if (AuthorFilterComboBox.SelectedValue != null && AuthorFilterComboBox.SelectedValue is int authorId && authorId > 0)
            {
                query = query.Where(b => b.AuthorId == authorId);
            }

            // Фильтр по жанру
            if (GenreFilterComboBox.SelectedValue != null && GenreFilterComboBox.SelectedValue is int genreId && genreId > 0)
            {
                query = query.Where(b => b.GenreId == genreId);
            }

            BooksGrid.ItemsSource = query.ToList();
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditBookWindow(_context);
            if (window.ShowDialog() == true)
            {
                LoadBooks();
            }
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksGrid.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для редактирования");
                return;
            }

            var window = new AddEditBookWindow(_context, selectedBook);
            if (window.ShowDialog() == true)
            {
                LoadBooks();
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksGrid.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для удаления");
                return;
            }

            if (MessageBox.Show("Удалить книгу?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Books.Remove(selectedBook);
                _context.SaveChanges();
                LoadBooks();
            }
        }

        private void ManageAuthors_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuthorsWindow(_context);
            window.ShowDialog();
            LoadData();
        }

        private void ManageGenres_Click(object sender, RoutedEventArgs e)
        {
            var window = new GenresWindow(_context);
            window.ShowDialog();
            LoadData();
        }
    }
}