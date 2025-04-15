using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace TempusVelit.Assets
{
    public class HiddenMenuProperty : INotifyPropertyChanged
    {
        private int width; 
        private Visibility isHidden;
        private string pathData;

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public Visibility IsHidden
        {
            get
            {
                return isHidden;
            }
            set
            {
                isHidden = value;
                OnPropertyChanged(nameof(IsHidden));
            }
        }

        public string PathData
        {
            get
            {
                return pathData;
            }
            set
            {
                pathData = value;
                OnPropertyChanged(nameof(PathData));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
