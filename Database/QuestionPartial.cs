using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempusVelit.Database
{
    public partial class Question
    {
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
    }
}
