using System.Linq;
using System.Windows;
using LibraryApp13.Data;
using LibraryApp13.Models;

namespace LibraryApp13
{
    public partial class AuthorsWindow : Window
    {
        private AppDbContext _context;

        public AuthorsWindow(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            AuthorsGrid.ItemsSource = _context.Authors.ToList();
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditAuthorWindow(_context);
            if (window.ShowDialog() == true)
            {
                LoadAuthors();
            }
        }

        private void EditAuthor_Click(object sender, RoutedEventArgs e)
        {
            var selectedAuthor = AuthorsGrid.SelectedItem as Author;
            if (selectedAuthor == null)
            {
                MessageBox.Show("Выберите автора");
                return;
            }

            var window = new AddEditAuthorWindow(_context, selectedAuthor);
            if (window.ShowDialog() == true)
            {
                LoadAuthors();
            }
        }

        private void DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            var selectedAuthor = AuthorsGrid.SelectedItem as Author;
            if (selectedAuthor == null)
            {
                MessageBox.Show("Выберите автора");
                return;
            }

            if (MessageBox.Show("Удалить автора?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Authors.Remove(selectedAuthor);
                _context.SaveChanges();
                LoadAuthors();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}