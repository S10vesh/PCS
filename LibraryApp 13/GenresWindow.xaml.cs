using System.Linq;
using System.Windows;
using LibraryApp13.Data;
using LibraryApp13.Models;

namespace LibraryApp13
{
    public partial class GenresWindow : Window
    {
        private AppDbContext _context;

        public GenresWindow(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadGenres();
        }

        private void LoadGenres()
        {
            GenresGrid.ItemsSource = _context.Genres.ToList();
        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditGenreWindow(_context);
            if (window.ShowDialog() == true)
            {
                LoadGenres();
            }
        }

        private void EditGenre_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenre = GenresGrid.SelectedItem as Genre;
            if (selectedGenre == null)
            {
                MessageBox.Show("Выберите жанр");
                return;
            }

            var window = new AddEditGenreWindow(_context, selectedGenre);
            if (window.ShowDialog() == true)
            {
                LoadGenres();
            }
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenre = GenresGrid.SelectedItem as Genre;
            if (selectedGenre == null)
            {
                MessageBox.Show("Выберите жанр");
                return;
            }

            if (MessageBox.Show("Удалить жанр?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Genres.Remove(selectedGenre);
                _context.SaveChanges();
                LoadGenres();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}