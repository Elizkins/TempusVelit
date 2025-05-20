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
using TempusVelit.Database;
using TempusVelit.Pages;
using TempusVelit.Properties;

namespace TempusVelit
{
    /// <summary>
    /// Логика взаимодействия для MainNavigationWindow.xaml
    /// </summary>
    public partial class MainNavigationWindow : NavigationWindow
    {
        public static NavigationService MainNavigationService { get; set; }

        public MainNavigationWindow()
        {
            InitializeComponent();

            MainNavigationService = NavigationService;

            User user = TempusVelitData.Context.User.Where(u => u.Email == Settings.Default.Email && u.PasswordHash == Settings.Default.Password).FirstOrDefault();
            if (user != null)
            {
                this.NavigationService.Navigate(new MainPage(user));
            }
        }
    }
}
