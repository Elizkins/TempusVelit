using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempusVelit.Database
{
    public partial class ControlTask
    {
        public IEnumerable<Question> FirstStageQuestions
        {
            get { return Questions.Where(q => q.StageID == 1); }
        }

        public IEnumerable<Question> SecondStageQuestions
        {
            get { return Questions.Where(q => q.StageID == 2); }
        }

        public Question ThirdStageQuestion
        {
            get { return Questions.First(q => q.StageID == 3); }
        }
    }
}
