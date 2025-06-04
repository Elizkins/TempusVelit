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

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для AchievementRoom.xaml
    /// </summary>
    public partial class AchievementRoom : Page
    {
        public AchievementRoom()
        {
            InitializeComponent();

            User.OrderNumber = 0;
            lvFirstUsers.ItemsSource = TempusVelitData.Context.Users.OrderByDescending(u => u.PointCount).ToList().GetRange(0, 3);
            lvLastUsers.ItemsSource = TempusVelitData.Context.Users.OrderByDescending(u => u.PointCount).ToList().GetRange(3, 7);
            lvAch.ItemsSource = TempusVelitData.Context.Achievements.ToList();
            this.DataContext = MainPage.User;
        }

        private void ListViewPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                };
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    }
}
