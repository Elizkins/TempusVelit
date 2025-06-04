using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для TextBoxWithPlaceholder.xaml
    /// </summary>
    public partial class TextBoxWithPlaceholder : UserControl
    {


        public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register("Placeholder", typeof(string), typeof(TextBoxWithPlaceholder));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register("Color", typeof(Brush), typeof(TextBoxWithPlaceholder));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWithPlaceholder));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(Geometry), typeof(TextBoxWithPlaceholder));


        public Geometry Icon
        {
            get
            {
                return (Geometry)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }

        public int CaretIndex
        {
            get
            {
                return entry.CaretIndex;
            }
            set
            {
                entry.CaretIndex = value;
            }
        }

        public int SelectionStart
        {
            get
            {
                return entry.SelectionStart;
            }
            set
            {
                entry.SelectionStart = value;
            }
        }

        public int SelectionLength
        {
            get
            {
                return entry.SelectionLength;
            }
            set
            {
                entry.SelectionLength = value;
            }
        }

        public event EventHandler TextChanged;


        public TextBoxWithPlaceholder()
        {
            InitializeComponent();
        }


        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            placeholder.Visibility = Visibility.Collapsed;
        }

        private void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox?.Text))
            {
                placeholder.Visibility = Visibility.Visible;
            }
        }

        private void RemovePlaceholderMB(object sender, MouseButtonEventArgs e)
        {
            entry.Focus();
        }

        private void EntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if ((e.Source as TextBox).Text.Length != 0)
            {
                RemovePlaceholder(sender, e);
            }
            TextChanged?.Invoke(sender, e);
        }
    }
}
