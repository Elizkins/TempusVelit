using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using TempusVelit.Database;

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для FirstStagePage.xaml
    /// </summary>
    public partial class FirstStagePage : Page
    {
        private DispatcherTimer timer;
        private TimeSpan elapsedTime;

        public FirstStagePage(ControlTask task)
        {
            InitializeComponent();
            this.DataContext = task;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;

            elapsedTime = TimeSpan.Zero;
            UpdateTimerText();

            timer.Start();

        }

        private void TimerTick(object sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            if (tbMin != null && tbSec != null)
            {
                tbMin.Text = elapsedTime.Minutes.ToString("00");
                tbSec.Text = elapsedTime.Seconds.ToString("00");
            }
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new ControlRoom());
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

        private void OpenPanel(object sender, MouseButtonEventArgs e)
        {
                      //TODO
        }
    }
}
