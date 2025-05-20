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
using System.Windows.Shapes;

namespace TempusVelit.Assets
{
    /// <summary>
    /// Логика взаимодействия для DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow(Message message)
        {
            InitializeComponent();

            this.DataContext = message;
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void OKButton(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
