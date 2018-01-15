using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.Exam
{
    public class ExamContext : EEContext
    {
        public ExamContext(string userId): base(userId, "DoExam")
        {
            this.selectContext = new ExamSelectionContext();
            this.processContext = new ExamProcessContext();
            this.stage  = StageType.Init;
        }

        
        public ExamSelectionContext selectContext;
        public ExamProcessContext processContext;
    }
}
