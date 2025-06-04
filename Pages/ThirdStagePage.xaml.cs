using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TempusVelit.Assets;
using TempusVelit.Database;

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для ThirdStagePage.xaml
    /// </summary>
    public partial class ThirdStagePage : Page
    {
        public class Employee
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string DepartmentName { get; set; }
        }


        private int counter = 0;
        private bool isCorrect = false;
        private DispatcherTimer timer;
        private TimeSpan elapsedTime;
        private ControlTask task;

        public ThirdStagePage(ControlTask task)
        {
            InitializeComponent();

            this.task = task;
            this.DataContext = this.task;


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;

            elapsedTime = TimeSpan.Zero;
            UpdateTimerText();

            timer.Start();

            var employees = new ObservableCollection<Employee>
{
    new Employee { FirstName = "John", LastName = "Smith", DepartmentName = "Sales" },
    new Employee { FirstName = "Sarah", LastName = "Johnson", DepartmentName = "Marketing" },
    new Employee { FirstName = "Michael", LastName = "Brown", DepartmentName = "IT" },
    new Employee { FirstName = "Emily", LastName = "Davis", DepartmentName = "Sales" },
    new Employee { FirstName = "David", LastName = "Wilson", DepartmentName = null }, // NULL для LEFT JOIN
    new Employee { FirstName = "Lisa", LastName = "Miller", DepartmentName = "HR" }
};

            // Устанавливаем источник данных для DataGrid
            dataGrid.ItemsSource = employees;
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
            if (new DialogWindow(new Message("Счет будет сброшен, вы уверены?", "НЕТ", "ОК")).ShowDialog() == true)
            {
                this.NavigationService.Navigate(new ControlRoom());
            }
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

        private void CreateRequestAnswer(object sender, MouseButtonEventArgs e)
        {
            this.counter++;
            var validator = new SqlQueryValidator();

            var userQuery = new TextRange(SqlEditor.Document.ContentStart, SqlEditor.Document.ContentEnd).Text;

            var result = validator.ValidateQuery(userQuery, task);

            if (result.IsValid)
            {
                Debug.WriteLine("Запрос верный!");

                this.Resources["PanelColor"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#95CF95");
                isCorrect = true;
                btnResult.Visibility = Visibility.Visible;

                timer.Stop();

                //CreateTableView();

                //Grid.SetColumn(-, 0);
                //Grid.SetColumnSpan(-, 3);
                //Grid.SetRow(-, 4);

                //gridMain.Children.Add(-);

                tbErrors.Text = "";

            }
            else
            {
                List<string> errors = new List<string>();
                this.Resources["PanelColor"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF3B30");


                foreach (var i in result.Errors)
                {
                    errors.Add(i);
                }
                if (result.MissingTables.Count > 0)
                    errors.Add($"не хватает таблиц ({string.Join(", ", result.MissingTables)})");
                if (result.MissingConditions.Count > 0)
                    errors.Add($"не хватает условий ({string.Join(", ", result.MissingConditions)})");
                if (result.MissingColumns.Count > 0)
                    errors.Add($"не хватает колонок ({string.Join(", ", result.MissingColumns)})");

                tbErrors.Text = $"Ошибки в запросе: {string.Join(", ", errors)}";
            }

            if (counter > 3)
            {
                btnResult.Visibility = Visibility.Visible;
            }
        }


        private void SqlEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            tbPlaceholder.Text = string.Empty;
        }

        private void OpenResultPage(object sender, MouseButtonEventArgs e)
        {
            var record = TempusVelitData.Context.UserControlTasks.Where(uct => uct.UserID == MainPage.User.UserID
                                                                                 && uct.TaskID == this.task.TaskID
                                                                                 && uct.StageID == 3).FirstOrDefault();
            if (record != null)
            {
                record.Score = isCorrect ? counter : 0;
                record.Time = new TimeSpan(0, Convert.ToInt32(tbMin.Text), Convert.ToInt32(tbSec.Text));
                TempusVelitData.Context.SaveChanges();
                this.NavigationService.Navigate(new ControlRoom(task, false));
            }
            else
            {
                UserControlTask userControlTask = new UserControlTask
                {
                    UserID = MainPage.User.UserID,
                    TaskID = this.task.TaskID,
                    StageID = 3,
                    Score = isCorrect ? counter : 0,
                    Time = new TimeSpan(0, Convert.ToInt32(tbMin.Text), Convert.ToInt32(tbSec.Text))
                };
                TempusVelitData.Context.UserControlTasks.Add(userControlTask);
                TempusVelitData.Context.SaveChanges();
                this.NavigationService.Navigate(new ControlRoom(task, true));
            }
        }
    }
}
