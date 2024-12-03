using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        private void TextBoxLogin_Changed(object sender, RoutedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (TextBoxLogin.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }
        }

        private void PasswordBox_Changed(object sender, RoutedEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Visible;
            if (PasswordBox.Password.Length > 0)
            {
                txtHintPassword.Visibility = Visibility.Hidden;
            }
        }

        private void DoublePasswordBox_Changed(object sender, RoutedEventArgs e)
        {
            txtHintPasswordDouble.Visibility = Visibility.Visible;
            if (DoublePasswordBox.Password.Length > 0)
            {
                txtHintPasswordDouble.Visibility = Visibility.Hidden;
            }
        }

        private void TextBoxFIO_Changed(object sender, RoutedEventArgs e)
        {
            txtHintFIO.Visibility = Visibility.Visible;
            if (TextBoxFIO.Text.Length > 0)
            {
                txtHintFIO.Visibility = Visibility.Hidden;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxLogin.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            DoublePasswordBox.Password = string.Empty;
            TextBoxFIO.Text = string.Empty;
            NavigationService?.Navigate(new AuthPage());
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password)
                || string.IsNullOrEmpty(DoublePasswordBox.Password) || string.IsNullOrEmpty(TextBoxFIO.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            using (var db = new Entities())
            {
                var user = db.User
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TextBoxLogin.Text);

                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже зарегистрирован.");
                    return;
                }

            }

            if (PasswordBox.Password.Length >= 6)
            {
                bool en = true;
                bool number = false;

                for (int i = 0; i < PasswordBox.Password.Length; i++)
                {
                    if ((PasswordBox.Password[i] >= 'А' && PasswordBox.Password[i] <= 'Я')
                        || (PasswordBox.Password[i] >= 'а' && PasswordBox.Password[i] <= 'я')
                        || (PasswordBox.Password[i] == 'ё' || PasswordBox.Password[i] == 'Ё'))
                        en = false;
                    if (PasswordBox.Password[i] >= '0' && PasswordBox.Password[i] <= '9') number = true;
                }

                if (!en)
                {
                    MessageBox.Show("Доступна только английская раскладка.");
                    return;

                }
                else if (!number)
                {
                    MessageBox.Show("Добавьте хотя бы одну цифру.");
                    return;
                }

            }
            else
            {
                MessageBox.Show("Пароль слишком короткий, минимм 6 символов.");
            }

            if (PasswordBox.Password != DoublePasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            string role = string.Empty;

            if (CmbRole.Text == "Администратор")
                role = "admin";
            else if (CmbRole.Text == "Пользователь")
                role = "user";

            using (var db = new Entities())
            {
                _121_Rulko.User newUser = new _121_Rulko.User
                {
                    FIO = TextBoxFIO.Text,
                    Login = TextBoxLogin.Text,
                    Password = GetHash(PasswordBox.Password),
                    Role = role
                };

                db.User.Add(newUser);
                db.SaveChanges();
            }
            MessageBox.Show("Регистрация прошла успешно!");
        }

    }
}
