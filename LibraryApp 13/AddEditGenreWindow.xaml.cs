using System;
using System.Windows;
using LibraryApp13.Data;
using LibraryApp13.Models;

namespace LibraryApp13
{
    public partial class AddEditGenreWindow : Window
    {
        private AppDbContext _context;
        private Genre _currentGenre;

        public AddEditGenreWindow(AppDbContext context, Genre genre = null)
        {
            InitializeComponent();
            _context = context;
            _currentGenre = genre ?? new Genre();

            if (genre != null)
            {
                Title = "Редактирование жанра";
                NameTextBox.Text = genre.Name;
                DescriptionTextBox.Text = genre.Description;
            }
            else
            {
                Title = "Добавление жанра";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Введите название жанра");
                    return;
                }

                // Заполняем жанр
                _currentGenre.Name = NameTextBox.Text;
                _currentGenre.Description = DescriptionTextBox.Text;

                if (_currentGenre.Id == 0)
                {
                    _context.Genres.Add(_currentGenre);
                }
                else
                {
                    _context.Entry(_currentGenre).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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