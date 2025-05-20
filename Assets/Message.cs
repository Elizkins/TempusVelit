using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempusVelit.Assets
{
    public class Message
    {
        public string HeadLine { get; set; }
        public string BackText { get; set; }
        public string OkText { get; set; }

        public Message(string headLine, string backText, string okText)
        {
            HeadLine = headLine;
            BackText = backText;
            OkText = okText;
        }
    }
}
