using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using TempusVelit.Assets;
using TempusVelit.Database;

namespace TempusVelit.Pages
{
    /// <summary>
    /// Логика взаимодействия для LessonPage.xaml
    /// </summary>
    public partial class LessonPage : Page
    {
        private List<string> tableHtmls = new List<string>();
        private List<Grid> buttons = new List<Grid>();
        private readonly Lesson lesson;
        private readonly bool isFromAdmin;
        private UserLesson userLesson;

        public LessonPage(Lesson lesson)
        {
            InitializeComponent();
            this.DataContext = lesson;

            if (TempusVelitData.Context.UserLessons.FirstOrDefault(us => us.UserID == MainPage.User.UserID && us.LessonID == lesson.LessonID) == null)
            {
                userLesson = new UserLesson
                {
                    UserID = MainPage.User.UserID,
                    LessonID = lesson.LessonID,
                    IsCompleted = false,
                };
                TempusVelitData.Context.UserLessons.Add(userLesson);
                TempusVelitData.Context.SaveChanges();
            }
            else
            {
                userLesson = TempusVelitData.Context.UserLessons.FirstOrDefault(us => us.UserID == MainPage.User.UserID && us.LessonID == lesson.LessonID);
            }

            FillStackPanel(lesson.Content);
            this.lesson = lesson;
        }

        public LessonPage(Lesson lesson, bool isFromAdmin)
        {
            InitializeComponent();
            this.isFromAdmin = isFromAdmin;
            this.lesson = lesson;

            this.DataContext = lesson;

            FillStackPanel(lesson.Content);
        }

        private void FillStackPanel(string lesson)
        {
            int pos = 0;
            lesson = lesson?.Replace("\n", "").Replace("\r", "");
            while (lesson != null && pos < lesson.Length)
            {
                if (lesson[pos] == '<')
                {
                    int tagStart = pos;
                    int tagEnd = lesson.IndexOf('>', pos);
                    if (tagEnd == -1) break;

                    string fullTag = lesson.Substring(tagStart, tagEnd - tagStart + 1);
                    string tagName = fullTag.Trim('<', '>', '/').Split(' ')[0].ToLower();

                    int contentStart = tagEnd + 1;
                    string closingTag = $"</{tagName}>";
                    int contentEnd = lesson.IndexOf(closingTag, contentStart);
                    if (contentEnd == -1) break;

                    string content = lesson.Substring(contentStart, contentEnd - contentStart);

                    switch (tagName)
                    {
                        case "h1":
                            spContent.Children.Add(CreateH1(content));
                            break;
                        case "h2":
                            spContent.Children.Add(CreateH2(content));
                            break;
                        case "h3":
                            spContent.Children.Add(CreateH3(content));
                            break;
                        case "img":
                            spContent.Children.Add(CreateImage(content));
                            break;
                        case "req":
                            spContent.Children.Add(CreateRequest(content));
                            break;
                        case "table":
                            tableHtmls.Add(content);
                            break;
                        default:
                            break;
                    }

                    pos = contentEnd + closingTag.Length;
                }
            }
            if (isFromAdmin)
            {
                return;
            }
            spContent.Children.Add(CreateCloseButton());
        }

        private Border CreateCloseButton()
        {
            Border outerBorder = new Border
            {
                Margin = new Thickness(0, 15, 10, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                BorderThickness = new Thickness(3),
                BorderBrush = (Brush)Application.Current.Resources["LightBaseColor"],
                Height = 70,
                CornerRadius = new CornerRadius(16),
                Background = Brushes.Transparent
            };

            DropShadowEffect shadowEffect = new DropShadowEffect
            {
                Color = Colors.Black,
                BlurRadius = 10,
                Opacity = 0.25,
                Direction = 310
            };
            outerBorder.Effect = shadowEffect;

            Border innerBorder = new Border
            {
                Background = (Brush)Application.Current.Resources["WhiteBaseColor"],
                CornerRadius = new CornerRadius(16),
                Padding = new Thickness(15, 0, 15, 0)
            };

            Label label = new Label
            {
                Content = "Следующее занятие",
                Foreground = (Brush)Application.Current.Resources["NormalBaseColor"],
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 30,
                FontFamily = (FontFamily)Application.Current.Resources["YsabeauInfant-Medium"]
            };

            innerBorder.Child = label;

            outerBorder.Child = innerBorder;

            outerBorder.PreviewMouseLeftButtonDown += OpenNextLesson;

            return outerBorder;
        }

        private void OpenNextLesson(object sender, MouseButtonEventArgs e)
        {

            if (TempusVelitData.Context.Lessons.Where(l => this.lesson.ModuleID == l.ModuleID && l.OrderNumber == this.lesson.OrderNumber + 1).FirstOrDefault() != null)
            {
                var lesson = TempusVelitData.Context.Lessons.Where(l => this.lesson.ModuleID == l.ModuleID && l.OrderNumber == this.lesson.OrderNumber + 1).FirstOrDefault();
                this.NavigationService.Navigate(new LessonPage(lesson));
            }
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            if (isFromAdmin)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                this.NavigationService.Navigate(new LearningModules());

            }
        }

        private TextBlock CreateH1(String text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 26,
                Foreground = (Brush)Application.Current.Resources["BlackBaseColor"],
                FontFamily = (FontFamily)Application.Current.Resources["YsabeauInfant-Medium"],
                Margin = new Thickness(0, 15, 0, 15)
            };

        }

        private Border CreateH2(String text)
        {
            Border outerBorder = new Border
            {
                CornerRadius = new CornerRadius(16),
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0, 15, 0, 15),
                Background = (Brush)Application.Current.Resources["NormalBaseColor"],
                Padding = new Thickness(4, 0, 4, 0)
            };

            Border innerBorder = new Border
            {
                Background = (Brush)Application.Current.Resources["WhiteBaseColor"],
                CornerRadius = new CornerRadius(16),
                Margin = new Thickness(-1)
            };

            TextBlock textBlock = new TextBlock
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 20,
                Foreground = (Brush)Application.Current.Resources["BlackBaseColor"],
                FontFamily = (FontFamily)Application.Current.Resources["Jost-Light"],
                Margin = new Thickness(15)
            };

            innerBorder.Child = textBlock;
            outerBorder.Child = innerBorder;
            return outerBorder;
        }

        private TextBlock CreateH3(String text)
        {
            return new TextBlock
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment = TextAlignment.Justify,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 20,
                Foreground = (Brush)Application.Current.Resources["BlackBaseColor"],
                FontFamily = (FontFamily)Application.Current.Resources["Jost-Light"]
            };

        }

        private Border CreateImage(String source)
        {
            return new Border
            {
                CornerRadius = new CornerRadius(16),
                BorderThickness = new Thickness(0),
                Height = 500,
                Margin = new Thickness(0, 15, 0, 15),
                Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(source)),
                    Stretch = Stretch.Fill
                }
            };
        }

        private Border CreateRequest(String text)
        {
            Border mainBorder = new Border
            {
                CornerRadius = new CornerRadius(16),
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0, 15, 0, 15),
                Background = (Brush)Application.Current.Resources["WhiteBaseColor"],
                Padding = new Thickness(30)
            };

            Grid contentGrid = new Grid();

            contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            TextBlock sqlTextBlock = new TextBlock
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 20,
                Foreground = (Brush)Application.Current.Resources["BlackBaseColor"],
                FontFamily = (FontFamily)Application.Current.Resources["Jost-Medium"]
            };
            Grid.SetColumn(sqlTextBlock, 0);

            Grid buttonGrid = new Grid
            {
                Margin = new Thickness(15, 0, 0, 0),
                Background = Brushes.Transparent
            };

            buttonGrid.PreviewMouseLeftButtonDown += CreateTableView;
            buttons.Add(buttonGrid);

            Path circlePath = new Path
            {
                Data = Geometry.Parse("M19 35.6668C28.205 35.6668 35.6667 28.2052 35.6667 19.0002C35.6667 9.79516 28.205 2.3335 19 2.3335C9.79504 2.3335 2.33337 9.79516 2.33337 19.0002C2.33337 28.2052 9.79504 35.6668 19 35.6668Z"),
                Stroke = (Brush)Application.Current.Resources["NormalBaseColor"],
                StrokeThickness = 3
            };

            Path trianglePath = new Path
            {
                Data = Geometry.Parse("M15.6667 13.2268L25.6667 19.0002L15.6667 24.7735V13.2268Z"),
                StrokeLineJoin = PenLineJoin.Round,
                Stroke = (Brush)Application.Current.Resources["NormalBaseColor"],
                StrokeThickness = 3
            };

            buttonGrid.Children.Add(circlePath);
            buttonGrid.Children.Add(trianglePath);
            Grid.SetColumn(buttonGrid, 1);

            contentGrid.Children.Add(sqlTextBlock);
            contentGrid.Children.Add(buttonGrid);

            mainBorder.Child = contentGrid;

            return mainBorder;
        }

        private void CreateTableView(object sender, MouseButtonEventArgs e)
        {

            int buttonIndex = buttons.IndexOf(sender as Grid);

            Grid mainGrid = new Grid();

            #region dataGrid
            DataGrid dataGrid = new DataGrid();
            dataGrid.Style = (Style)Application.Current.Resources["DataGridMainStyle"];

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(tableHtmls[buttonIndex]);
            DataTable table = new DataTable();
            var headerRow = htmlDoc.DocumentNode.SelectSingleNode("//tr");
            foreach (var headerCell in headerRow.Elements("th"))
            {
                table.Columns.Add(headerCell.InnerText.Trim());
            }
            var dataRows = htmlDoc.DocumentNode.SelectNodes("//tr[position()>1]");
            foreach (var row in dataRows)
            {
                var dataRow = table.NewRow();
                var cells = row.Elements("td").ToList();
                for (int i = 0; i < cells.Count(); i++)
                {
                    dataRow[i] = cells[i].InnerText.Trim();
                }
                table.Rows.Add(dataRow);
            }
            dataGrid.ItemsSource = table.DefaultView;
            dataGrid.PreviewMouseWheel += DataGridPreviewMouseWheel;
            #endregion

            #region closeButton
            StackPanel stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(15, 30, 30, 15),
                Background = (Brush)Application.Current.Resources["WhiteBaseColor"]
            };
            Grid iconGrid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 11),
                Background = Brushes.Transparent
            };

            iconGrid.PreviewMouseLeftButtonDown += CloseTableView;

            Path outerCircle = new Path
            {
                Data = Geometry.Parse("M29.8731 5.12416C28.2495 3.4974 26.3207 2.20733 24.1972 1.32798C22.0738 0.448642 19.7976 -0.00265023 17.4993 1.17077e-05C7.83465 1.17077e-05 0 7.83498 0 17.5C0 22.3325 1.95976 26.7086 5.12686 29.8758C6.75046 31.5026 8.67933 32.7927 10.8028 33.672C12.9262 34.5514 15.2024 35.0026 17.5007 35C27.1654 35 35 27.165 35 17.5C35 12.6675 33.0402 8.2914 29.8731 5.12416ZM27.6247 27.62C26.2961 28.9508 24.7179 30.0063 22.9807 30.7259C21.2434 31.4456 19.3812 31.8153 17.5007 31.8138C9.59318 31.8138 3.18314 25.4035 3.18314 17.4956C3.18166 15.6151 3.55134 13.7528 4.27097 12.0155C4.9906 10.2781 6.04604 8.6999 7.37679 7.37127C8.70519 6.04064 10.2831 4.98526 12.0201 4.2656C13.7571 3.54595 15.6191 3.17617 17.4993 3.17745C25.4054 3.17745 31.8154 9.58775 31.8154 17.4942C31.8167 19.3744 31.4469 21.2365 30.7273 22.9735C30.0077 24.7106 28.9523 26.2886 27.6218 27.6171L27.6247 27.62Z"),
                Fill = (Brush)Application.Current.Resources["NormalBaseColor"]
            };
            Path crossIcon = new Path
            {
                Data = Geometry.Parse("M19.7492 17.5015L25.3704 11.8801C25.6476 11.5778 25.7975 11.1801 25.7886 10.7701C25.7797 10.36 25.6128 9.96919 25.3227 9.67924C25.0325 9.3893 24.6416 9.22262 24.2316 9.21399C23.8215 9.20537 23.424 9.35546 23.1219 9.63295L23.1234 9.63149L17.5022 15.2529L11.881 9.63149C11.5788 9.3542 11.1811 9.20436 10.7711 9.21325C10.361 9.22215 9.97022 9.38908 9.68029 9.67921C9.39036 9.96934 9.22369 10.3602 9.21506 10.7703C9.20644 11.1804 9.35653 11.578 9.634 11.8801L9.63255 11.8786L15.2537 17.5L9.63255 23.1214C9.47439 23.2665 9.34723 23.4421 9.25872 23.6377C9.17022 23.8332 9.12219 24.0446 9.11754 24.2592C9.11289 24.4738 9.1517 24.6871 9.23165 24.8863C9.3116 25.0855 9.43103 25.2664 9.58275 25.4183C9.73446 25.5701 9.91533 25.6896 10.1145 25.7697C10.3136 25.8498 10.5269 25.8888 10.7415 25.8843C10.956 25.8797 11.1675 25.8318 11.3631 25.7435C11.5587 25.6551 11.7344 25.528 11.8796 25.37L17.5022 19.7471L23.1234 25.3685C23.2685 25.5267 23.4441 25.6538 23.6396 25.7423C23.8351 25.8309 24.0465 25.8789 24.2611 25.8835C24.4757 25.8882 24.689 25.8494 24.8882 25.7694C25.0874 25.6895 25.2683 25.57 25.4201 25.4183C25.572 25.2666 25.6915 25.0857 25.7716 24.8866C25.8517 24.6874 25.8906 24.4741 25.8861 24.2595C25.8816 24.045 25.8337 23.8335 25.7453 23.6379C25.6569 23.4423 25.5299 23.2666 25.3718 23.1214L19.7492 17.5015Z"),
                Fill = (Brush)Application.Current.Resources["NormalBaseColor"]
            };
            iconGrid.Children.Add(outerCircle);
            iconGrid.Children.Add(crossIcon);
            Rectangle separator = new Rectangle
            {
                Height = 3,
                Fill = (Brush)Application.Current.Resources["LightBaseColor"]
            };
            stackPanel.Children.Add(iconGrid);
            stackPanel.Children.Add(separator);
            #endregion

            mainGrid.Children.Add(dataGrid);
            mainGrid.Children.Add(stackPanel);

            var border = ((sender as Grid).Parent as Grid).Parent as Border;
            int borderIndex = spContent.Children.IndexOf(border);
            spContent.Children.Insert(borderIndex + 1, mainGrid);
            //spContent.Children.Add(mainGrid);

            if (isFromAdmin)
            {
                return;
            }
            if (!MainPage.User.UserAchievements.Any(ua => ua.AchievementID == 10))
            {
                TempusVelitData.Context.UserAchievements.Add(new UserAchievement
                {
                    UserID = MainPage.User.UserID,
                    AchievementID = 10
                });

                MainPage.User.PointCount += 50;
                TempusVelitData.Context.SaveChanges();
                new DialogWindow(new Message("Вы получили достижение", "", "ОК")).ShowDialog();
            }
        }

        private void DataGridPreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

        private void CloseTableView(object sender, MouseButtonEventArgs e)
        {
            var tablePanel = ((sender as Grid).Parent as StackPanel).Parent as Grid;
            spContent.Children.Remove(tablePanel);
        }

        private void GetPageEnd(object sender, ScrollChangedEventArgs e)
        {
            if (isFromAdmin)
            {
                return;
            }

            var scrollViewer = (ScrollViewer)sender;

            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 1)
            {
                var controlTask = TempusVelitData.Context.ControlTasks.FirstOrDefault(ct => ct.LessonID == lesson.LessonID);

                if (controlTask != null)
                {
                    int taskId = controlTask.TaskID;

                    var tasksToAdd = new List<UserControlTask>();

                    for (int i = 1; i <= 3; i++)
                    {
                        bool exists = TempusVelitData.Context.UserControlTasks
                            .Any(uct => uct.UserID == MainPage.User.UserID
                                     && uct.TaskID == taskId
                                     && uct.StageID == i);

                        if (!exists)
                        {
                            tasksToAdd.Add(new UserControlTask()
                            {
                                UserID = MainPage.User.UserID,
                                TaskID = taskId,
                                StageID = i,
                            });
                        }
                    }
                    if (tasksToAdd.Any())
                    {
                        TempusVelitData.Context.UserControlTasks.AddRange(tasksToAdd);
                    }
                }
                userLesson.IsCompleted = true;
                userLesson.CompletedDate = DateTime.Now;

                MainPage.User.PointCount += 10;

                TempusVelitData.Context.SaveChanges();

                int moduleId = lesson.ModuleID;
                var module = TempusVelitData.Context.LearningModules.Find(moduleId);

                if (module != null &&
                    MainPage.User.UserLessons.Count(ul => ul.Lesson.ModuleID == moduleId) == module.Lessons.Count)
                {
                    var moduleAchievements = new Dictionary<int, int>
                    {
                        { 1, 6 },  // Module 1 -> Achievement 6
                        { 2, 8 },  // Module 2 -> Achievement 8
                        { 3, 7 }   // Module 3 -> Achievement 7
                    };

                    if (moduleAchievements.TryGetValue(moduleId, out int achievementId))
                    {
                        if (!MainPage.User.UserAchievements.Any(ua => ua.AchievementID == achievementId))
                        {
                            TempusVelitData.Context.UserAchievements.Add(new UserAchievement()
                            {
                                UserID = MainPage.User.UserID,
                                AchievementID = achievementId,
                            });

                            MainPage.User.PointCount += 50;
                        }
                    }

                    var allModules = TempusVelitData.Context.LearningModules.ToList();
                    bool allModulesCompleted = allModules.All(m =>
                        MainPage.User.UserLessons.Count(ul => ul.Lesson.ModuleID == m.ModuleID) == m.Lessons.Count);

                    if (allModulesCompleted &&
                        !MainPage.User.UserAchievements.Any(ua => ua.AchievementID == 9))
                    {
                        TempusVelitData.Context.UserAchievements.Add(new UserAchievement
                        {
                            UserID = MainPage.User.UserID,
                            AchievementID = 9
                        });

                        MainPage.User.PointCount += 50;
                        TempusVelitData.Context.SaveChanges();

                        new DialogWindow(new Message("Вы получили достижение", "", "ОК")).ShowDialog();
                    }

                    
                }
            }
        }
    }
}
