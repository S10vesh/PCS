using System;
using System.Windows;
using LibraryApp13.Data;
using LibraryApp13.Models;

namespace LibraryApp13
{
    public partial class AddEditAuthorWindow : Window
    {
        private AppDbContext _context;
        private Author _currentAuthor;

        public AddEditAuthorWindow(AppDbContext context, Author author = null)
        {
            InitializeComponent();
            _context = context;
            _currentAuthor = author ?? new Author();

            if (author != null)
            {
                Title = "Редактирование автора";
                FirstNameTextBox.Text = author.FirstName;
                LastNameTextBox.Text = author.LastName;
                BirthDatePicker.SelectedDate = author.BirthDate;
                CountryTextBox.Text = author.Country;
            }
            else
            {
                Title = "Добавление автора";
                BirthDatePicker.SelectedDate = DateTime.Now.AddYears(-30);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
                {
                    MessageBox.Show("Введите имя");
                    return;
                }

                if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
                {
                    MessageBox.Show("Введите фамилию");
                    return;
                }

                if (BirthDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату рождения");
                    return;
                }

                // Заполняем автора
                _currentAuthor.FirstName = FirstNameTextBox.Text;
                _currentAuthor.LastName = LastNameTextBox.Text;
                _currentAuthor.BirthDate = BirthDatePicker.SelectedDate.Value;
                _currentAuthor.Country = CountryTextBox.Text;

                if (_currentAuthor.Id == 0)
                {
                    _context.Authors.Add(_currentAuthor);
                }
                else
                {
                    _context.Entry(_currentAuthor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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