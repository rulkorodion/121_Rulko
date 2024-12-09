using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _121_Rulko.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        private _121_Rulko.User _currentUser = new _121_Rulko.User();
        public AddUserPage(_121_Rulko.User selectedUser)
        {
            InitializeComponent();

            if(selectedUser != null)
            {
                _currentUser = selectedUser;
            }

            DataContext = _currentUser;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
                errors.AppendLine("Укажите логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password))
                errors.AppendLine("Укажите пароль!");
            if ((_currentUser.Role == null) || (CmbRole.Text == ""))
                errors.AppendLine("Выберите роль!");
            else
                _currentUser.Role = CmbRole.Text;
            if (string.IsNullOrWhiteSpace(_currentUser.FIO))
                errors.AppendLine("Укажите Ф.И.О.!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentUser.ID == 0)
                Entities.GetContext().User.Add(_currentUser);

            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService?.Navigate(new Admin());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxLogin.Text = string.Empty;
            TextBoxPassword.Text = string.Empty;
            TextBoxFIO.Text = string.Empty;
            TextBoxPhoto.Text = string.Empty;
            NavigationService?.Navigate(new Admin());
        }

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (TextBoxLogin.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }
        }

        private void TextBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Visible;
            if (TextBoxPassword.Text.Length > 0)
            {
                txtHintPassword.Visibility = Visibility.Hidden;
            }
        }

        private void TextBoxFIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintFIO.Visibility = Visibility.Visible;
            if (TextBoxFIO.Text.Length > 0)
            {
                txtHintFIO.Visibility = Visibility.Hidden;
            }
        }

        private void TextBoxPhoto_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintPhoto.Visibility = Visibility.Visible;
            if(TextBoxPhoto.Text.Length > 0)
            {
                txtHintPhoto.Visibility = Visibility.Hidden;
            }
        }
    }
}
