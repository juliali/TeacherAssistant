using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.Exam
{    

    public class ExamItem
    {
        public int index;        
        public string question;        
        public string answer;
        public string interpret;
        public string answerType;
        public string answerRegex;
        public string askbackMessage;
        public int score = -1;
    }
    public class ExamInfo
    {
        public string courseName;
        public string columnName;
        public string description;
        public int askBackLimitTimes = 1;
        public List<ExamItem> items;
        public ExamItemAnswerDisplayType dispType;
        public QuitType quitType;
        public string summerizerClassName;
        public string quitMessage;
        public string restartMessage;      
    }

    public class ExamProcessContext: ISelectionContext
    {
        public int askbackTime = 0;
        public int index = 0;
        public List<string> collectedUserAnswer = new List<string>();
    }
}
