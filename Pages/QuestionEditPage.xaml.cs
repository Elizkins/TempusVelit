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
using TempusVelit.Database;

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для QuestionEditPage.xaml
    /// </summary>
    public partial class QuestionEditPage : Page
    {
        private readonly Question question;

        public QuestionEditPage(Question question, bool isThirdStage)
        {
            InitializeComponent();

            this.question = question;
            this.DataContext = question;

            if (isThirdStage)
            {
                thirdPanel.Visibility = Visibility.Visible;
                thirdPanel.DataContext = question.ControlTask;
                lvAnswersList.Visibility = Visibility.Collapsed;
            }
            else
            {
                lvAnswersList.ItemsSource = question.Answers;
            }
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
