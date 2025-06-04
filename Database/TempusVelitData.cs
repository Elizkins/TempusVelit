using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempusVelit.Database
{
    public class TempusVelitData
    {
        public static TempusVelitEntities Context = new TempusVelitEntities();

        public static string GetDenclensionString(int number, string nominative, string genitive, string plural)
        {
            string[] titles = new[] { nominative, genitive, plural };
            int[] cases = new[] { 2, 0, 1, 1, 1, 2 };
            return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
        }

    }
}
