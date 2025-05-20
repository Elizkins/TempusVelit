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
using System.Windows.Threading;
using TempusVelit.Database;

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для LearningModules.xaml
    /// </summary>
    public partial class LearningModules : Page
    {
        public LearningModules()
        {
            InitializeComponent();

            progressBar.DataContext = TempusVelitData.Context.Lesson.ToList();
            lvModules.ItemsSource = TempusVelitData.Context.LearningModule.ToList();
            lvModuleProgress.ItemsSource = TempusVelitData.Context.LearningModule.ToList();
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

        private void OpenLessonsList(object sender, MouseButtonEventArgs e)
        {
            overlay.DataContext = (sender as Border).DataContext as LearningModule;
            overlay.Visibility = Visibility.Visible;
        }

        private void CloseLessonsList(object sender, MouseButtonEventArgs e)
        {
            overlay.Visibility = Visibility.Collapsed;

        }

        private void OpenLesson(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new LessonPage((sender as StackPanel).DataContext as Lesson));
        }
    }
}
