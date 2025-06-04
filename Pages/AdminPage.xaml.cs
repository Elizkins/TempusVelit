using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();

            TempusVelitData.Context.Lessons.Load();
            lessonList.ItemsSource = TempusVelitData.Context.Lessons.Local;
        }

        private void OpenLesson(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new LessonEditPage((sender as Border).DataContext as Lesson));
        }

        private void DeleteLesson(object sender, MouseButtonEventArgs e)
        {
            Lesson lesson = (sender as Border).DataContext as Lesson;

            foreach (var controlTask in lesson.ControlTasks)
            {
                foreach (var question in controlTask.Questions)
                {
                    TempusVelitData.Context.Answers.RemoveRange(question.Answers);
                }
                TempusVelitData.Context.Questions.RemoveRange(controlTask.Questions);
                TempusVelitData.Context.UserControlTasks.RemoveRange(controlTask.UserControlTasks);
            }
            TempusVelitData.Context.ControlTasks.RemoveRange(lesson.ControlTasks);
            TempusVelitData.Context.UserLessons.RemoveRange(lesson.UserLessons);

            TempusVelitData.Context.Lessons.Remove(lesson);
            TempusVelitData.Context.SaveChanges();
        }

        private void CreateNewLesson(object sender, MouseButtonEventArgs e)
        {
            Lesson lesson = new Lesson
            {
                LessonName = "Тема",
                Content = "<h1>Заголовок</h1><h2>Акцентный текст</h2><h3>Основной текст</h3><req>Request</req><table><table><tr><th>greeting</th></tr><tr><td>Hello, SQL World!</td></tr></table></table><img>https://image.jpg</img>",
                ModuleID = 1,
                OrderNumber = TempusVelitData.Context.Lessons.Where(l => l.ModuleID == 1).Count() + 1,
            };
            TempusVelitData.Context.Lessons.Add(lesson);
            TempusVelitData.Context.SaveChanges();

            ControlTask controlTask = new ControlTask
            {
                ImageSource = string.Empty,
                LessonID = lesson.LessonID,
                RequiredTables = "Table",
                RequiredConditions = "Condition",
                RequiredColumns = "Column"
            };
            TempusVelitData.Context.ControlTasks.Add(controlTask);
            TempusVelitData.Context.SaveChanges();

            CreateQuestionsWithAnswers(10, 1, controlTask.TaskID);
            CreateQuestionsWithAnswers(5, 2, controlTask.TaskID);

            Question finalQuestion = new Question
            {
                QuestionText = "Вопрос",
                StageID = 3,
                TaskID = controlTask.TaskID
            };

            TempusVelitData.Context.Questions.Add(finalQuestion);
            TempusVelitData.Context.SaveChanges();

            TempusVelitData.Context.SaveChanges();

            this.NavigationService.Navigate(new LessonEditPage(lesson));
        }

        private void CreateQuestionsWithAnswers(int count, int stageId, int taskId)
        {
            for (int i = 0; i < count; i++)
            {
                var question = new Question
                {
                    QuestionText = "Вопрос",
                    StageID = stageId,
                    TaskID = taskId
                };

                TempusVelitData.Context.Questions.Add(question);
                TempusVelitData.Context.SaveChanges();

                for (int j = 0; j < 4; j++)
                {
                    TempusVelitData.Context.Answers.Add(new Answer
                    {
                        AnswerText = "Ответ",
                        QuestionID = question.QuestionID,
                        IsCorrect = false
                    });
                }
                TempusVelitData.Context.SaveChanges();
            }
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
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
