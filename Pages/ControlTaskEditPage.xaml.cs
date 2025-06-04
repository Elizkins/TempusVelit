using Microsoft.Win32;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для ControlTaskEditPage.xaml
    /// </summary>
    public partial class ControlTaskEditPage : Page
    {
        private ControlTask controlTask;
        private string imagePath;

        public ControlTaskEditPage(ControlTask controlTask)
        {
            InitializeComponent();

            this.controlTask = controlTask;
            this.DataContext = controlTask;
            lvQuestionList.ItemsSource = controlTask.Questions;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            if (imagePath != null)
                controlTask.ImageSource = imagePath;
            TempusVelitData.Context.SaveChanges();
            this.NavigationService.GoBack();
        }

        private void OpenQuestion(object sender, MouseButtonEventArgs e)
        {
            Question question = (sender as Border).DataContext as Question;
            if (question.StageID == 3)
            {
                this.NavigationService.Navigate(new QuestionEditPage(question, true));
            }
            else
            {
                this.NavigationService.Navigate(new QuestionEditPage(question, false));
            }
        }

        private void LoadImage(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png"
            };
            if (dlg.ShowDialog() == true)
            {
                imagePath = dlg.FileName;
                imagePath = PostImageToServer(imagePath);
            }
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
    }
}
