using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
using TempusVelit.Assets;
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
        private ControlTask task;

        public FirstStagePage(ControlTask task)
        {
            InitializeComponent();

            Question.OrderNumber = 0;

            this.task = task;
            foreach (var question in this.task.Questions)
            {
                foreach (var answer in question.Answers)
                {
                    answer.IsSelected = false;
                    answer.IsNeedToHightlight = false;
                    answer.IsEnabled = true;
                }
            }
            this.DataContext = this.task;


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

        private void OpenPanel(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();


            int correctCount = 0;

            foreach (var question in this.task.Questions.Where(q => q.StageID == 1).ToList())
            {
                bool isQuestionCorrect = true;

                if(question.Answers.Count == 0)
                {
                    isQuestionCorrect = false;
                }

                foreach (var answer in question.Answers)
                {
                    if (answer.IsSelected)
                    {
                        if (answer.IsCorrect != true)
                        {
                            isQuestionCorrect = false;
                        }
                        answer.IsNeedToHightlight = true;
                        answer.IsSelected = false;
                    }
                    else
                    {
                        if (answer.IsCorrect == true)
                        {
                            isQuestionCorrect = false;
                        }
                    }
                    answer.IsEnabled = false;
                }
                if (isQuestionCorrect)
                {
                    correctCount++;
                }
            }

            tbCounter.Text = correctCount.ToString();

            notDonePanel.Visibility = Visibility.Collapsed;
            donePanel.Visibility = Visibility.Visible;
        }

        private void OpenNextStage(object sender, MouseButtonEventArgs e)
        {
            var record = TempusVelitData.Context.UserControlTasks.Where(uct => uct.UserID == MainPage.User.UserID
                                                                                 && uct.TaskID == this.task.TaskID
                                                                                 && uct.StageID == 1).FirstOrDefault();
            if (record != null)
            {
                record.Score = Convert.ToInt32(tbCounter.Text);
                record.Time = new TimeSpan(0, Convert.ToInt32(tbMin.Text), Convert.ToInt32(tbSec.Text));
            }
            else
            {
                UserControlTask userControlTask = new UserControlTask
                {
                    UserID = MainPage.User.UserID,
                    TaskID = this.task.TaskID,
                    StageID = 1,
                    Score = Convert.ToInt32(tbCounter.Text),
                    Time = new TimeSpan(0, Convert.ToInt32(tbMin.Text), Convert.ToInt32(tbSec.Text))
                };
                TempusVelitData.Context.UserControlTasks.Add(userControlTask);
            }
            TempusVelitData.Context.SaveChanges();
            this.NavigationService.Navigate(new SecondStagePage(this.task));
        }
    }
}
