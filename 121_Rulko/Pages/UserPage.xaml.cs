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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            CmbSorting.SelectedIndex = 0;
            CheckUser.IsChecked = false;
            UpdateUsers();
        }

        private void UpdateUsers()
        {
            if (ListUser == null || CmbSorting == null || TextBoxFIO == null || CheckUser == null)
            {
                return;
            }

            var currentUsers = Entities.GetContext().User.ToList();

            if (!string.IsNullOrWhiteSpace(TextBoxFIO.Text))
            {
                currentUsers = currentUsers.Where(x => x.FIO != null && x.FIO.ToLower().Contains(TextBoxFIO.Text.ToLower())).ToList();
            }

            if (CheckUser.IsChecked == true)
            {
                currentUsers = currentUsers.Where(x => x.Role != null && x.Role.Contains("Пользователь")).ToList();
            }

            if (CmbSorting.SelectedIndex == 0)
            {
                ListUser.ItemsSource = currentUsers.OrderBy(x => x.FIO).ToList();
            }
            else
            {
                ListUser.ItemsSource = currentUsers.OrderByDescending(x => x.FIO).ToList();
            }
        }

        private void TextBoxFIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void CmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void CheckUser_Checked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void CheckUser_UnChecked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            TextBoxFIO.Text = string.Empty;
            CmbSorting.SelectedIndex = 0;
            CheckUser.IsChecked = false;
            UpdateUsers();
        }
    }
}
