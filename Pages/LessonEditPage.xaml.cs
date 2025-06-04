using HtmlAgilityPack;
using Microsoft.Win32;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using static TempusVelit.Database.Lesson;

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для LessonEditPage.xaml
    /// </summary>
    public partial class LessonEditPage : Page
    {
        private Lesson lesson;
        ObservableCollection<LessonContent> contentList;

        public LessonEditPage(Lesson lesson)
        {
            InitializeComponent();

            this.lesson = lesson;

            if (lesson.ModuleID == 1)
            {
                rb1Module.IsChecked = true;
            }
            else if (lesson.ModuleID == 2)
            {
                rb2Module.IsChecked = true;
            }
            else if (lesson.ModuleID == 3)
            {
                rb3Module.IsChecked = true;
            }

            lesson.LoadList();
            contentList = lesson.ContentList;
            lvContentList.ItemsSource = contentList;
            this.DataContext = lesson;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            if (new DialogWindow(new Message("Вы сохранили изменения?", "НЕТ", "ОК")).ShowDialog() == true)
            {
                TempusVelitData.Context.SaveChanges();
                this.NavigationService.GoBack();
            }
        }

        private void AddH1Click(object sender, RoutedEventArgs e)
        {
            contentList.Add(new LessonContent
            {
                Tag = "h1",
                Text = "Заголовок"
            });
        }

        private void AddH2Click(object sender, RoutedEventArgs e)
        {
            contentList.Add(new LessonContent
            {
                Tag = "h2",
                Text = "Акцентный текст"
            });
        }

        private void AddH3Click(object sender, RoutedEventArgs e)
        {
            contentList.Add(new LessonContent
            {
                Tag = "h3",
                Text = "Основной текст"
            });
        }

        private void AddSqlExampleClick(object sender, RoutedEventArgs e)
        {
            contentList.Add(new LessonContent
            {
                Tag = "req",
                Text = "Request"
            });
            contentList.Add(new LessonContent
            {
                Tag = "table",
                Text = "<table><tr><th>greeting</th></tr><tr><td>Hello, SQL World!</td></tr></table>"
            });
        }

        private void AddImageClick(object sender, RoutedEventArgs e)
        {
            string imagePath = string.Empty;
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png"
            };
            if (dlg.ShowDialog() == true)
            {
                imagePath = dlg.FileName;
                imagePath = PostImageToServer(imagePath);
            }

            contentList.Add(new LessonContent
            {
                Tag = "img",
                Text = imagePath
            });
        }

        public string PostImageToServer(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                var client = new RestClient();

                var request = new RestRequest("http://elizasem-001-site1.ktempurl.com/api/ImageUpload", Method.Post);
                request.AddFile("file", imagePath);

                RestResponse response = client.Execute(request);

                string imageAddress = "http://elizasem-001-site1.ktempurl.com/api/ImageUpload/" + response.Content.Replace("\"", "");

                return imageAddress;
            }
            return null;
        }

        private void PreviewClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LessonPage(lesson, true));
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nameBox.Text))
                lesson.LessonName = nameBox.Text;
            if (rb1Module.IsChecked == true)
            {
                lesson.ModuleID = 1;
            }
            else if (rb2Module.IsChecked == true)
            {
                lesson.ModuleID = 2;
            }
            else if (rb3Module.IsChecked == true)
            {
                lesson.ModuleID = 3;
            }

            bool isOkey = true;

            string content = string.Empty;

            for (int i = 0; i < contentList.Count; i++)
            {
                var item = contentList[i];
                if (item.Tag == "req" && contentList[i + 1].Tag != "table" ||
                    item.Tag == "table" && contentList[i - 1].Tag != "req")
                {
                    new DialogWindow(new Message("После запроса должна следовать таблица", "", "ОК")).ShowDialog();
                    isOkey = false;
                    break;
                }
                else if (item.Tag == "table" && !IsValidHtmlTable(item.Text))
                {
                    new DialogWindow(new Message("Некорректная таблица", "", "ОК")).ShowDialog();
                    isOkey = false;
                    break;
                }
                else
                {
                    content += item;
                }
            }
            if (isOkey)
            {
                lesson.Content = content;
            }
        }

        private bool IsValidHtmlTable(string tableHtml)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(tableHtml);

                var rows = doc.DocumentNode.SelectNodes("//tr");
                if (rows == null || rows.Count == 0)
                    return false;

                int? expectedColumns = null;

                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("th|td");
                    if (cells == null || cells.Count == 0)
                        return false;

                    if (expectedColumns == null)
                        expectedColumns = cells.Count;
                    else if (cells.Count != expectedColumns)
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void DeleteTag(object sender, MouseButtonEventArgs e)
        {
            contentList.Remove((sender as Border).DataContext as LessonContent);
        }

        private void RaiseTag(object sender, MouseButtonEventArgs e)
        {
            var lessonPart = (sender as Border).DataContext as LessonContent;

            if (lessonPart == null) return;

            int currentIndex = contentList.IndexOf(lessonPart);
            if (currentIndex > 0)
            {
                contentList.Move(currentIndex, currentIndex - 1);
            }
        }

        private void LowerTag(object sender, MouseButtonEventArgs e)
        {
            var lessonPart = (sender as Border).DataContext as LessonContent;

            if (lessonPart == null) return;

            int currentIndex = contentList.IndexOf(lessonPart);
            if (currentIndex < contentList.Count - 1)
            {
                contentList.Move(currentIndex, currentIndex + 1);
            }
        }

        private void OpenControlTaskEditPage(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ControlTaskEditPage(TempusVelitData.Context.ControlTasks.Where(ct => ct.LessonID == lesson.LessonID).FirstOrDefault()));
        }
    }
}
