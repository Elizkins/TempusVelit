using RestSharp;
using System;
using System.Collections.Generic;
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
using TempusVelit.Assets;
using TempusVelit.Database;

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для PasswordRenewPage.xaml
    /// </summary>
    public partial class PasswordRenewPage : Page
    {
        private string code;

        public PasswordRenewPage()
        {
            InitializeComponent();
        }

        private void OpenLogInPage(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new LogInPage());
        }

        private void ChangePassword(object sender, MouseButtonEventArgs e)
        {
            if (code == codeEntry.Text)
            {
                try
                {
                    User user = TempusVelitData.Context.Users.Where(u => u.Email == emailEntry.Text).FirstOrDefault();
                    if (user != null)
                    { 
                        user.PasswordHash = passwordEntry.Text;
                    }
                    TempusVelitData.Context.SaveChanges();
                    new DialogWindow(new Message("Ваш пароль изменен", "", "ОК")).ShowDialog();
                    NavigationService.Navigate(new LogInPage());
                }
                catch (Exception)
                {
                    new DialogWindow(new Message("Произошла ошибка", "", "ОК")).ShowDialog();
                    return;
                }

            }
            else
            {
                new DialogWindow(new Message("Данные не корректны", "", "ОК")).ShowDialog();
            }
        }

        private void SendEmail(object sender, MouseButtonEventArgs e)
        {
            try
            {
                User user = TempusVelitData.Context.Users.Where(u => u.Email == emailEntry.Text).FirstOrDefault();
                if (user == null)
                {
                    new DialogWindow(new Message("Такого аккаунта не существует", "", "ОК")).ShowDialog();
                    return;
                }
                code = PostEmailLetter(emailEntry.Text);
                new DialogWindow(new Message("Письмо отправлено на почту", "", "ОК")).ShowDialog();
            }
            catch (Exception)
            {
                new DialogWindow(new Message("Произошла ошибка", "", "ОК")).ShowDialog();
                return;
            }
        }

        public string PostEmailLetter(string email)
        {
            var client = new RestClient();

            var request = new RestRequest("http://elizasem-001-site1.ktempurl.com/api/EmailSend", Method.Post);

            request.AlwaysMultipartFormData = true;
            request.AddParameter("email", email);

            RestResponse response = client.Execute(request);

            string code = response.Content.Replace("\"", "");

            return code;
        }
    }
}
