using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TempusVelit.Database;
using TempusVelit.Pages;

namespace TempusVelit.Assets
{
    public class TaskToProcentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ControlTask task)
            {
                var stages = task.UserControlTasks.Where(u => u.UserID == MainPage.User.UserID).ToList();
                int counter = 0;

                foreach (var stage in stages)
                {
                    if (stage.Time != null && stage.Score != null)
                    {
                        double bal;
                        TimeSpan timeLimit;

                        switch (stage.StageID)
                        {
                            case 1:
                                bal = (double)stage.Score;
                                timeLimit = new TimeSpan(0, 7, 0);
                                break;
                            case 2:
                                bal = (double)(stage.Score * 10);
                                timeLimit = new TimeSpan(0, 7, 0);
                                break;
                            case 3:
                                switch ((int)stage.Score)
                                {
                                    case 1:
                                        bal = 40;
                                        break;
                                    case 2:
                                        bal = 30;
                                        break;
                                    case 3:
                                        bal = 20;
                                        break;
                                    default:
                                        bal = 10;
                                        break;
                                }
                                timeLimit = new TimeSpan(0, 10, 0);
                                break;
                            default:
                                continue;
                        }

                        if (stage.Time <= timeLimit)
                        {
                            bal *= 1.2;
                        }
                        else if (stage.Time <= new TimeSpan(0, 15, 0))
                        {
                            bal *= 1;
                        }
                        else
                        {
                            bal *= 0.8;
                        }

                        counter += (int)bal;
                    }
                }
                if (counter > 100)
                {
                    return null;
                }
                return $"{counter}%";
            }
            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
