using System;
using System.Linq;
using System.Windows;
using LibraryApp13.Data;
using LibraryApp13.Models;

namespace LibraryApp13
{
    public partial class AddEditBookWindow : Window
    {
        private AppDbContext _context;
        private Book _currentBook;

        public AddEditBookWindow(AppDbContext context, Book book = null)
        {
            InitializeComponent();
            _context = context;
            _currentBook = book ?? new Book();

            LoadComboBoxes();

            if (book != null)
            {
                Title = "Редактирование книги";
                TitleTextBox.Text = book.Title;
                YearTextBox.Text = book.PublishYear.ToString();
                ISBNTextBox.Text = book.ISBN;
                QuantityTextBox.Text = book.QuantityInStock.ToString();
                AuthorComboBox.SelectedValue = book.AuthorId;
                GenreComboBox.SelectedValue = book.GenreId;
            }
            else
            {
                Title = "Добавление книги";
            }
        }

        private void LoadComboBoxes()
        {
            var authors = _context.Authors.ToList()
                .Select(a => new { a.Id, FullName = $"{a.FirstName} {a.LastName}" })
                .ToList();
            AuthorComboBox.ItemsSource = authors;

            var genres = _context.Genres.ToList();
            GenreComboBox.ItemsSource = genres;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
                {
                    MessageBox.Show("Введите название книги");
                    return;
                }

                if (AuthorComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Выберите автора");
                    return;
                }

                if (GenreComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Выберите жанр");
                    return;
                }

                if (!int.TryParse(YearTextBox.Text, out int year) || year < 1800 || year > DateTime.Now.Year)
                {
                    MessageBox.Show("Введите корректный год (1800-" + DateTime.Now.Year + ")");
                    return;
                }

                if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Введите корректное количество");
                    return;
                }

                // Заполняем книгу
                _currentBook.Title = TitleTextBox.Text;
                _currentBook.PublishYear = year;
                _currentBook.ISBN = ISBNTextBox.Text;
                _currentBook.QuantityInStock = quantity;
                _currentBook.AuthorId = (int)AuthorComboBox.SelectedValue;
                _currentBook.GenreId = (int)GenreComboBox.SelectedValue;

                if (_currentBook.Id == 0)
                {
                    // Новая книга
                    _context.Books.Add(_currentBook);
                }
                else
                {
                    // Существующая книга - обновляем
                    _context.Entry(_currentBook).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}