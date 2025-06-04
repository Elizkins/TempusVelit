using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempusVelit.Database
{
    public partial class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static int _orderNumber = 0;

        public static int OrderNumber
        {
            get
            {
                return ++_orderNumber;
            }
            set
            {
                _orderNumber = value;
            }
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    OnPropertyChanged(nameof(Icon));
                }
            }
        }

        public string PointCountString
        {
            get => $"{PointCount} {TempusVelitData.GetDenclensionString(PointCount, "балл", "балла", "баллов")}";
        }

        public int Level
        {
            get => PointCount / 100;
        }

        public int LevelPoints
        {
            get => PointCount % 100;
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
