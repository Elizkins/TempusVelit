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

namespace TempusVelit.Assets
{
    /// <summary>
    /// Логика взаимодействия для MenuControl.xaml
    /// </summary>
    public partial class MenuControl : UserControl
    {
        private const string pathExpland = "M47.5625 24C47.5625 37.0126 37.0126 47.5625 24 47.5625C10.9874 47.5625 0.4375 37.0126 0.4375 24C0.4375 10.9874 10.9874 0.4375 24 0.4375C37.0126 0.4375 47.5625 10.9874 47.5625 24ZM23.4732 13.6506C23.3042 13.8183 23.1699 14.0176 23.0779 14.2371C22.9859 14.4567 22.9381 14.6922 22.9371 14.9303C22.9362 15.1683 22.9821 15.4043 23.0724 15.6245C23.1626 15.8448 23.2954 16.0452 23.463 16.2142L29.3899 22.1875H14.2578C13.7771 22.1875 13.3161 22.3785 12.9762 22.7184C12.6363 23.0583 12.4453 23.5193 12.4453 24C12.4453 24.4807 12.6363 24.9417 12.9762 25.2816C13.3161 25.6215 13.7771 25.8125 14.2578 25.8125H29.3899L23.463 31.7858C23.2954 31.955 23.1627 32.1555 23.0725 32.3759C22.9824 32.5963 22.9365 32.8324 22.9375 33.0705C22.9386 33.3087 22.9865 33.5443 23.0787 33.7639C23.1708 33.9835 23.3052 34.1828 23.4744 34.3505C23.6435 34.5182 23.844 34.6509 24.0645 34.741C24.2849 34.8312 24.5209 34.8771 24.7591 34.876C24.9972 34.875 25.2328 34.827 25.4525 34.7349C25.6721 34.6428 25.8714 34.5083 26.0391 34.3392L35.0325 25.2767C35.3694 24.9372 35.5584 24.4783 35.5584 24C35.5584 23.5217 35.3694 23.0628 35.0325 22.7233L26.0391 13.6608C25.8714 13.4915 25.6719 13.3568 25.4522 13.2646C25.2324 13.1725 24.9966 13.1245 24.7583 13.1236C24.5199 13.1226 24.2838 13.1687 24.0633 13.2591C23.8428 13.3496 23.6423 13.4826 23.4732 13.6506Z";
        private const string pathHide = "M0.4375 24C0.4375 37.0126 10.9874 47.5625 24 47.5625C37.0126 47.5625 47.5625 37.0126 47.5625 24C47.5625 10.9874 37.0126 0.4375 24 0.4375C10.9874 0.4375 0.4375 10.9874 0.4375 24ZM24.5268 13.6506C24.6958 13.8183 24.8301 14.0176 24.9221 14.2371C25.0141 14.4567 25.0619 14.6922 25.0629 14.9303C25.0638 15.1683 25.0179 15.4043 24.9276 15.6245C24.8374 15.8448 24.7046 16.0452 24.537 16.2142L18.6101 22.1875H33.7422C34.2229 22.1875 34.6839 22.3785 35.0238 22.7184C35.3637 23.0583 35.5547 23.5193 35.5547 24C35.5547 24.4807 35.3637 24.9417 35.0238 25.2816C34.6839 25.6215 34.2229 25.8125 33.7422 25.8125H18.6101L24.537 31.7858C24.7046 31.955 24.8373 32.1555 24.9275 32.3759C25.0176 32.5963 25.0635 32.8324 25.0625 33.0705C25.0614 33.3087 25.0135 33.5443 24.9213 33.7639C24.8292 33.9835 24.6948 34.1828 24.5256 34.3505C24.3565 34.5182 24.156 34.6509 23.9355 34.741C23.7151 34.8312 23.4791 34.8771 23.2409 34.876C23.0028 34.875 22.7672 34.827 22.5475 34.7349C22.3279 34.6428 22.1286 34.5083 21.9609 34.3392L12.9675 25.2767C12.6306 24.9372 12.4416 24.4783 12.4416 24C12.4416 23.5217 12.6306 23.0628 12.9675 22.7233L21.9609 13.6608C22.1286 13.4915 22.3281 13.3568 22.5478 13.2646C22.7676 13.1725 23.0034 13.1245 23.2417 13.1236C23.4801 13.1226 23.7162 13.1687 23.9367 13.2591C24.1572 13.3496 24.3577 13.4826 24.5268 13.6506Z";

        private HiddenMenuProperty menuProperty;
        private bool flag = true;

        public MenuControl()
        {
            InitializeComponent();

            menuProperty = new HiddenMenuProperty()
            {
                Width = 400,
                IsHidden = Visibility.Visible,
                PathData = pathHide
            };

            DataContext = menuProperty;
        }

        private void HideMenu(object sender, MouseButtonEventArgs e)
        {
            if (flag)
            {
                menuProperty.Width = 100;
                menuProperty.IsHidden = Visibility.Collapsed;
                menuProperty.PathData = pathExpland;
                flag = false;
            }
            else
            {
                menuProperty.Width = 400;
                menuProperty.IsHidden = Visibility.Visible;
                menuProperty.PathData = pathHide;
                flag = true;
            }
        }

        private void GoToProfile(object sender, MouseButtonEventArgs e)
        {
            SetActiveBorder(ProfileBorder);
        }

        private void GoToLearningModules(object sender, MouseButtonEventArgs e)
        {
            SetActiveBorder(LearningBorder);
        }

        private void GoToControlRoom(object sender, MouseButtonEventArgs e)
        {
            SetActiveBorder(ControlBorder);
        }

        private void GoToAchievementRoom(object sender, MouseButtonEventArgs e)
        {
            SetActiveBorder(AchievmentBorder);
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
