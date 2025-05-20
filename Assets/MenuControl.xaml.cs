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
using TempusVelit.Pages;

namespace TempusVelit.Assets
{
    /// <summary>
    /// Логика взаимодействия для MenuControl.xaml
    /// </summary>
    public partial class MenuControl : UserControl
    {
        private const string pathExpland = "M0.6875 21C0.6875 32.2178 9.78223 41.3125 21 41.3125C32.2178 41.3125 41.3125 32.2178 41.3125 21C41.3125 9.78223 32.2178 0.6875 21 0.6875C9.78223 0.6875 0.6875 9.78223 0.6875 21ZM21.4541 12.0781C21.5998 12.2226 21.7156 12.3944 21.7949 12.5837C21.8742 12.773 21.9155 12.9761 21.9163 13.1813C21.9171 13.3865 21.8775 13.5899 21.7997 13.7798C21.7219 13.9697 21.6074 14.1424 21.4629 14.2881L16.3535 19.4375H29.3984C29.8128 19.4375 30.2103 19.6021 30.5033 19.8951C30.7963 20.1882 30.9609 20.5856 30.9609 21C30.9609 21.4144 30.7963 21.8118 30.5033 22.1049C30.2103 22.3979 29.8128 22.5625 29.3984 22.5625H16.3535L21.4629 27.7119C21.6074 27.8577 21.7218 28.0306 21.7995 28.2206C21.8773 28.4106 21.9168 28.6141 21.9159 28.8194C21.915 29.0247 21.8737 29.2278 21.7943 29.4172C21.7149 29.6065 21.5989 29.7783 21.4531 29.9229C21.3073 30.0674 21.1345 30.1818 20.9444 30.2595C20.7544 30.3372 20.5509 30.3768 20.3456 30.3759C20.1403 30.375 19.9372 30.3336 19.7479 30.2542C19.5586 30.1748 19.3867 30.0589 19.2422 29.9131L11.4893 22.1006C11.1988 21.8079 11.0359 21.4123 11.0359 21C11.0359 20.5877 11.1988 20.1921 11.4893 19.8994L19.2422 12.0869C19.3867 11.9409 19.5587 11.8249 19.7481 11.7454C19.9376 11.6659 20.1409 11.6246 20.3463 11.6238C20.5518 11.6229 20.7554 11.6627 20.9455 11.7406C21.1355 11.8186 21.3084 11.9333 21.4541 12.0781Z";
        private const string pathHide = "M41.3125 21C41.3125 32.2178 32.2178 41.3125 21 41.3125C9.78223 41.3125 0.6875 32.2178 0.6875 21C0.6875 9.78223 9.78223 0.6875 21 0.6875C32.2178 0.6875 41.3125 9.78223 41.3125 21ZM20.5459 12.0781C20.4002 12.2226 20.2844 12.3944 20.2051 12.5837C20.1258 12.773 20.0845 12.9761 20.0837 13.1813C20.0829 13.3865 20.1225 13.5899 20.2003 13.7798C20.2781 13.9697 20.3926 14.1424 20.5371 14.2881L25.6465 19.4375H12.6016C12.1872 19.4375 11.7897 19.6021 11.4967 19.8951C11.2037 20.1882 11.0391 20.5856 11.0391 21C11.0391 21.4144 11.2037 21.8118 11.4967 22.1049C11.7897 22.3979 12.1872 22.5625 12.6016 22.5625H25.6465L20.5371 27.7119C20.3926 27.8577 20.2782 28.0306 20.2005 28.2206C20.1227 28.4106 20.0832 28.6141 20.0841 28.8194C20.085 29.0247 20.1263 29.2278 20.2057 29.4172C20.2851 29.6065 20.4011 29.7783 20.5469 29.9229C20.6927 30.0674 20.8655 30.1818 21.0556 30.2595C21.2456 30.3372 21.4491 30.3768 21.6544 30.3759C21.8597 30.375 22.0628 30.3336 22.2521 30.2542C22.4414 30.1748 22.6133 30.0589 22.7578 29.9131L30.5107 22.1006C30.8012 21.8079 30.9641 21.4123 30.9641 21C30.9641 20.5877 30.8012 20.1921 30.5107 19.8994L22.7578 12.0869C22.6133 11.9409 22.4413 11.8249 22.2519 11.7454C22.0624 11.6659 21.8591 11.6246 21.6537 11.6238C21.4482 11.6229 21.2446 11.6627 21.0545 11.7406C20.8645 11.8186 20.6916 11.9333 20.5459 12.0781Z";

        private HiddenMenuProperty menuProperty;
        private bool flag = true;

        public static NavigationService NavigationService { get; set; }

        public MenuControl()
        {
            InitializeComponent();

            menuProperty = new HiddenMenuProperty()
            {
                Width = 360,
                IsHidden = Visibility.Visible,
                PathData = pathHide
            };

            DataContext = menuProperty;
        }

        private void HideMenu(object sender, MouseButtonEventArgs e)
        {
            if (flag)
            {
                menuProperty.Width = 90;
                menuProperty.IsHidden = Visibility.Collapsed;
                menuProperty.PathData = pathExpland;
                flag = false;
            }
            else
            {
                menuProperty.Width = 360;
                menuProperty.IsHidden = Visibility.Visible;
                menuProperty.PathData = pathHide;
                flag = true;
            }
        }

        private void GoToProfile(object sender, MouseButtonEventArgs e)
        {
            SetActiveBorder(ProfileBorder);
            NavigationService.Navigate(new Profile());
        }

        private void GoToLearningModules(object sender, MouseButtonEventArgs e)
        {
            SetActiveBorder(LearningBorder);
            NavigationService.Navigate(new LearningModules());
        }

        private void GoToControlRoom(object sender, MouseButtonEventArgs e)
        {
            SetActiveBorder(ControlBorder);
            NavigationService.Navigate(new ControlRoom());
        }

        private void GoToAchievementRoom(object sender, MouseButtonEventArgs e)
        {
            SetActiveBorder(AchievmentBorder);
            NavigationService.Navigate(new AchievementRoom());
        }

        private void SetActiveBorder(Border activeBorder)
        {
            activeBorder.Background = this.FindResource("WhiteBaseColor") as SolidColorBrush;

            foreach (var border in new[] { ProfileBorder, LearningBorder, ControlBorder, AchievmentBorder })
            {
                if (border != activeBorder)
                {
                    border.Background = Brushes.Transparent;
                }
            }

            //
            for (int i = 0; i < 6; i++)
            {
                (MainGrid.Children[i] as Border).CornerRadius = new CornerRadius(0, 0, 0, 0);
            }

            int borderIndex = MainGrid.Children.IndexOf(activeBorder.Parent as Border);
            (MainGrid.Children[borderIndex - 1] as Border).CornerRadius = new CornerRadius(0, 0, 35, 0);
            (MainGrid.Children[borderIndex + 1] as Border).CornerRadius = new CornerRadius(0, 35, 0, 0);

            Border topBorder = MainGrid.Children[0] as Border;
            if (topBorder != null)
            {
                CornerRadius currentRadius = topBorder.CornerRadius;
                currentRadius.TopRight = 35;
                topBorder.CornerRadius = currentRadius;
            }

        }
    }
}
