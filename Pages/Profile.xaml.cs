using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using TempusVelit.Assets;
using TempusVelit.Database;
using TempusVelit.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public Profile()
        {
            InitializeComponent();

            this.DataContext = MainPage.User;
            imageList.ItemsSource = new string[]{ "http://elizasem-001-site1.ktempurl.com/api/ImageUpload/user_1.png",
                                                  "http://elizasem-001-site1.ktempurl.com/api/ImageUpload/user_2.png",
                                                  "http://elizasem-001-site1.ktempurl.com/api/ImageUpload/user_3.png",
                                                  "http://elizasem-001-site1.ktempurl.com/api/ImageUpload/user_4.png" };

            lvAch.ItemsSource = MainPage.User.UserAchievements;

        }

        private void GoOut(object sender, MouseButtonEventArgs e)
        {
            Settings.Default.Email = null;
            Settings.Default.Password = null;
            Settings.Default.Save();

            MainPage.User = null;

            MainNavigationWindow.MainNavigationService.Navigate(new LogInPage());
        }

        private void ClosePanel(object sender, MouseButtonEventArgs e)
        {
            overlay.Visibility = Visibility.Collapsed;
        }

        private void OpenPanel(object sender, MouseButtonEventArgs e)
        {
            overlay.Visibility = Visibility.Visible;

        }

        private void ChangeInfo(object sender, MouseButtonEventArgs e)
        {
            MainPage.User.FirstName = firstNameBox.Text;
            MainPage.User.LastName = lastNameBox.Text;
            MainPage.User.Phone = phoneBox.Text;
            MainPage.User.Email = emailEntry.Text;

            try
            {
                TempusVelitData.Context.SaveChanges();
                new DialogWindow(new Message("Ваши данные изменены", "", "ОК")).ShowDialog();
                overlay.Visibility = Visibility.Collapsed;

            }
            catch (Exception)
            {
                new DialogWindow(new Message("Проверьте корректность данных", "", "ОК")).ShowDialog();
            }
        }

        private void OpenImagePanel(object sender, MouseButtonEventArgs e)
        {
            imagePanel.Visibility = Visibility.Visible;
        }

        private void CloseImagePanel(object sender, MouseButtonEventArgs e)
        {
            imagePanel.Visibility = Visibility.Collapsed;
        }

        private void ChangeIcon(object sender, MouseButtonEventArgs e)
        {
            string url = (sender as Border).DataContext as string;

            MainPage.User.Icon = url;
            TempusVelitData.Context.SaveChanges();

            imagePanel.Visibility = Visibility.Collapsed;
        }
    }

}
