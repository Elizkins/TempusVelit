using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TempusVelit.Database
{
    public partial class Lesson : INotifyPropertyChanged
    {
        

        public class LessonContent
        {
            public static ObservableCollection<string> ContentTypes { get; } = new ObservableCollection<string>
            {
                "h1",
                "h2",
                "h3",
                "req",
                "table",
                "img"
            };

            public string Tag { get; set; }

            public string Text { get; set; }

            public override string ToString()
            {
                return $"<{Tag}>{Text}</{Tag}>";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private int _moduleID;

        private ObservableCollection<LessonContent> _contentList;

        public int ModuleID
        {
            get => _moduleID;
            set
            {
                if (_moduleID != value)
                {
                    _moduleID = value;
                    OnPropertyChanged(nameof(ModuleID));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<LessonContent> ContentList
        {
            get
            {
                return _contentList;
            }
        }

        public void LoadList()
        {
            _contentList = new ObservableCollection<LessonContent>();
            int pos = 0;
            string lesson = Content?.Replace("\n", "").Replace("\r", "");
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

                    _contentList.Add(new LessonContent
                    {
                        Tag = tagName,
                        Text = content,
                    });

                    pos = contentEnd + closingTag.Length;
                }
            }
        }
    }
}
