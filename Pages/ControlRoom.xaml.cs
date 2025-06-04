using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для ControlRoom.xaml
    /// </summary>
    public partial class ControlRoom : Page
    {
        public ControlRoom()
        {
            InitializeComponent();
            var controlTaskIds = MainPage.User.UserControlTasks.Select(s => s.TaskID).Distinct().ToList();
            lvTasks.ItemsSource = TempusVelitData.Context.ControlTasks.Where(c => controlTaskIds.Contains(c.TaskID)).ToList();

        }

        public ControlRoom(ControlTask task, bool isFirst)
        {
            InitializeComponent();
            var controlTaskIds = MainPage.User.UserControlTasks.Select(s => s.TaskID).Distinct().ToList();
            lvTasks.ItemsSource = TempusVelitData.Context.ControlTasks.Where(c => controlTaskIds.Contains(c.TaskID)).ToList();
            int counter = SetDataContext(task);

            if (isFirst)
            {
                MainPage.User.PointCount += (int)(counter * 0.1);
                TempusVelitData.Context.SaveChanges();
            }

            overlay.Visibility = Visibility.Visible;
        }

        private void OpenFirstStage(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is StackPanel || (e.OriginalSource as FrameworkElement)?.Parent is StackPanel || ((e.OriginalSource as FrameworkElement)?.Parent as Grid)?.Parent is StackPanel)
            {
                return;
            }
            this.NavigationService.Navigate(new FirstStagePage((sender as Border).DataContext as ControlTask));
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

        private void CloseResultPanel(object sender, MouseButtonEventArgs e)
        {
            overlay.Visibility = Visibility.Collapsed;
        }

        private void OpenResultPanel(object sender, MouseButtonEventArgs e)
        {
            var task = (sender as StackPanel).DataContext as ControlTask;
            SetDataContext(task);

            overlay.Visibility = Visibility.Visible;
        }

        private int SetDataContext(ControlTask task)
        {
            titleBox.Text = task.Lesson.LessonName;
            firstStack.DataContext = task.UserControlTasks.Where(u => u.UserID == MainPage.User.UserID && u.StageID == 1).FirstOrDefault();
            secondStack.DataContext = task.UserControlTasks.Where(u => u.UserID == MainPage.User.UserID && u.StageID == 2).FirstOrDefault();
            thirdStack.DataContext = task.UserControlTasks.Where(u => u.UserID == MainPage.User.UserID && u.StageID == 3).FirstOrDefault();
            var stages = task.UserControlTasks.Where(u => u.UserID == MainPage.User.UserID).ToList();
            int counter = 0;
            TimeSpan time = TimeSpan.Zero;
            foreach (var stage in stages)
            {
                if(stage.Time != null && stage.Score != null)
                {
                    time += (TimeSpan)stage.Time;

                    double bal;
                    TimeSpan timeLimit;

                    switch (stage.StageID)
                    {
                        case 1:
                            bal = (double)stage.Score;
                            timeLimit = new TimeSpan(0, 7, 0);
                            break;
                        case 2:
                            bal = (double)(stage.Score * 10);
                            timeLimit = new TimeSpan(0, 7, 0);
                            break;
                        case 3:
                            switch ((int)stage.Score)
                            {
                                case 0:
                                    bal = 0;
                                    break;
                                case 1:
                                    bal = 40;
                                    break;
                                case 2:
                                    bal = 30;
                                    break;
                                case 3:
                                    bal = 20;
                                    break;
                                default:
                                    bal = 10;
                                    break;
                            }
                            timeLimit = new TimeSpan(0, 10, 0);
                            break;
                        default:
                            continue;
                    }

                    if (stage.Time <= timeLimit)
                    {
                        bal *= 1.2;
                    }
                    else if (stage.Time <= new TimeSpan(0, 15, 0))
                    {
                        bal *= 1;
                    }
                    else
                    {
                        bal *= 0.8;
                    }

                    counter += (int)bal;
                }
            }

            tbCounter.Text = counter > 100 ? "100" : counter.ToString();
            tbMin.Text = time.ToString("mm");
            tbSec.Text = time.ToString("ss");

            return counter;
        }
    }
}
