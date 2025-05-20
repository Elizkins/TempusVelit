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
using TempusVelit.Properties;

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        private bool passwordOpen = false;

        private const string pathExpland = "M13.9996 0C17.009 0 19.7038 1.43645 21.8131 3.0752C23.671 4.51864 25.174 6.19614 26.1686 7.43848L26.5631 7.94238L26.5641 7.94336C27.4989 9.16612 27.5002 10.8351 26.5641 12.0576H26.5631C25.5961 13.3199 23.9364 15.2752 21.8131 16.9248C19.7038 18.5635 17.009 20 13.9996 20C10.9905 19.9999 8.29631 18.5634 6.18713 16.9248C4.06375 15.2751 2.40312 13.3199 1.43616 12.0576L1.43713 12.0566C0.981945 11.4675 0.734009 10.7447 0.734009 10C0.734009 9.25507 0.981644 8.53163 1.43713 7.94238H1.43616C2.40312 6.68009 4.06375 4.72492 6.18713 3.0752C8.29631 1.43657 10.9905 0.000121101 13.9996 0ZM16.4996 10C16.4996 8.61929 15.3803 7.5 13.9996 7.5C12.6191 7.50021 11.4996 8.61941 11.4996 10C11.4996 11.3806 12.6191 12.4998 13.9996 12.5C15.3803 12.5 16.4996 11.3807 16.4996 10ZM3.23401 10C3.23401 10.1442 3.26971 10.2855 3.33752 10.4111L3.41565 10.5303L3.42151 10.5371L3.78674 11.0039C4.7043 12.1494 6.06785 13.6663 7.72034 14.9502C9.62311 16.4285 11.7817 17.4999 13.9996 17.5C16.2176 17.5 18.3761 16.4285 20.2789 14.9502C22.1677 13.4827 23.6794 11.7111 24.5787 10.5371L24.6608 10.4111C24.824 10.108 24.7966 9.74812 24.5778 9.46191V9.46094C23.6783 8.28697 22.1668 6.51658 20.2789 5.0498C18.3761 3.57145 16.2176 2.5 13.9996 2.5C11.7817 2.50012 9.62311 3.57148 7.72034 5.0498C5.83172 6.5172 4.32075 8.28901 3.42151 9.46289L3.41565 9.46973C3.29793 9.62143 3.23401 9.80797 3.23401 10ZM18.9996 10C18.9996 12.7614 16.7611 15 13.9996 15C11.2384 14.9998 8.99963 12.7613 8.99963 10C8.99963 7.2387 11.2384 5.00021 13.9996 5C16.7611 5 18.9996 7.23858 18.9996 10Z";
        private const string pathHide = "M25.5596 0.676813C26.0142 0.157461 26.8038 0.105083 27.3233 0.559625C27.8426 1.01426 27.895 1.80382 27.4404 2.3233L26.2695 1.29791C26.3393 1.359 26.4158 1.42636 26.5 1.50005C27.4407 2.32319 27.4399 2.32381 27.4395 2.32427V2.32525C27.4388 2.32597 27.4373 2.32633 27.4365 2.3272C27.435 2.32893 27.4337 2.33172 27.4317 2.33404C27.4275 2.3387 27.4223 2.34467 27.416 2.35162C27.4033 2.36569 27.3856 2.38429 27.3643 2.40728C27.3215 2.45333 27.261 2.51669 27.1836 2.59478C27.0286 2.75115 26.8044 2.96726 26.5117 3.22271C26.3163 3.39327 26.0888 3.58095 25.832 3.7813L27.4531 5.69146L27.5313 5.79302C27.8901 6.31501 27.802 7.03449 27.3086 7.45318C26.8152 7.87174 26.0914 7.8413 25.6348 7.4024L25.5469 7.30865L23.7432 5.18365C22.5773 5.85615 21.14 6.51643 19.4424 6.99908L20.1856 9.22955L20.2197 9.35357C20.3581 9.97158 20.0092 10.6059 19.3955 10.8106C18.7815 11.0152 18.1204 10.7174 17.8604 10.1397L17.8145 10.0206L16.9844 7.53033C16.0489 7.66927 15.0538 7.75005 14 7.75005C12.9382 7.75005 11.9359 7.66832 10.9942 7.5274L10.1895 10.0108C9.97653 10.6674 9.27092 11.0273 8.61427 10.8145C7.95767 10.6016 7.59773 9.89597 7.81056 9.23931L8.5381 6.9942C6.84916 6.51208 5.41822 5.85449 4.25685 5.18463L2.45314 7.30963C2.00624 7.83576 1.21662 7.90004 0.69044 7.45318C0.164303 7.00629 0.100024 6.21666 0.546885 5.69048L2.167 3.7813C1.91051 3.58113 1.68356 3.39313 1.48829 3.22271C1.19561 2.96726 0.971395 2.75115 0.816416 2.59478C0.739024 2.51669 0.678549 2.45333 0.635752 2.40728C0.614388 2.38429 0.596675 2.36569 0.583994 2.35162C0.577729 2.34467 0.572505 2.3387 0.568369 2.33404C0.566308 2.33172 0.565011 2.32893 0.563487 2.3272C0.56272 2.32633 0.561187 2.32597 0.560557 2.32525V2.32427C0.560153 2.32381 0.559289 2.32319 1.50001 1.50005C1.58423 1.42636 1.66067 1.359 1.73048 1.29791L0.55958 2.3233C0.105038 1.80382 0.157416 1.01426 0.676768 0.559625C1.19608 0.105224 1.98477 0.157912 2.43946 0.676813L2.44142 0.677789C2.4458 0.682654 2.45438 0.691761 2.46681 0.705133C2.49205 0.732298 2.53386 0.77655 2.59181 0.835016C2.70775 0.952004 2.88874 1.12688 3.13282 1.3399C3.49422 1.65528 3.99291 2.05299 4.62403 2.47369C4.66319 2.49632 4.70112 2.52159 4.73829 2.54888C4.92778 2.67284 5.12871 2.79916 5.34083 2.92486C6.60458 3.6737 8.2654 4.42306 10.3057 4.86138C10.3621 4.86888 10.4183 4.88096 10.4746 4.89654C11.5478 5.11569 12.7234 5.25005 14 5.25005C15.2855 5.25005 16.4686 5.11357 17.5479 4.89166C17.5563 4.88969 17.5648 4.88758 17.5733 4.8858C19.6689 4.45155 21.3706 3.68843 22.6592 2.92486C23.6386 2.34449 24.3784 1.76644 24.8672 1.3399C25.1113 1.12688 25.2923 0.952004 25.4082 0.835016C25.4662 0.77655 25.508 0.732298 25.5332 0.705133C25.5456 0.691761 25.5542 0.682654 25.5586 0.677789L25.5596 0.676813ZM2.43946 0.675836H2.44044L2.43946 0.676813C2.43808 0.675272 2.43759 0.673979 2.43751 0.673883L2.43946 0.675836Z";


        public LogInPage()
        {
            InitializeComponent();
        }

        private void OpenLerningModules(object sender, MouseButtonEventArgs e)
        {
            User user = TempusVelitData.Context.User.Where(u => u.Email == emailEntry.Text).FirstOrDefault();
            if (user != null)
            {
                if (user.PasswordHash == passwordEntry.Text)
                {
                    this.NavigationService.Navigate(new MainPage(user));
                    Settings.Default.Email = user.Email;
                    Settings.Default.Password = user.PasswordHash;
                    Settings.Default.Save();
                }
                else
                {
                    new DialogWindow(new Message("Неверный пароль", "", "ОК")).ShowDialog();
                }
            }
            else
            {
                new DialogWindow(new Message("Такого аккаунта не существует", "", "ОК")).ShowDialog();
            }
        }

        private void OpenSingInPage(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new SingInPage());
        }

        //private void OpenPassword(object sender, MouseButtonEventArgs e)
        //{
        //    if (passwordOpen)
        //    {
        //        eye.Data = Geometry.Parse(pathExpland);
        //        //passwordBox.Visibility = Visibility.Visible;
        //        passwordOpen = false;
        //        //passwordBox.Password = passwordEntry.Text;
        //        //passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { passwordBox.Password.Length, 0 });
        //        //passwordBox.Focus();
        //    }
        //    else
        //    {
        //        eye.Data = Geometry.Parse(pathHide);
        //        //passwordBox.Visibility = Visibility.Collapsed;
        //        passwordOpen = true;
        //        //passwordEntry.Text = passwordBox.Password;
        //        //passwordEntry.SelectionStart = passwordEntry.Text.Length;
        //        //passwordEntry.Focus();
        //    }
        //}

        //private void AddNumberAndPasswordText(object sender, RoutedEventArgs e)
        //{

        //}

        //private void RemovePasswordText(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
