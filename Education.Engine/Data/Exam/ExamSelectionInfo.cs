using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.Exam
{
    public enum ExamItemGenerateType
    {
        RandomSelection,
        Overall
    }

    public enum ExamItemAnswerDisplayType
    {
        OnebyOne,
        AllInSummary
    }
    public enum QuitType
    {
        SummeryImmediately,
        CachedWithoutSummry,
        DispatchedWithoutSummry
    }

    public class ExamSelectionInfo
    {
        public string optionMessage;        
        public int askbackTimeLimitation = 1;                
        public string optionRegex;
        public string askbackMessage;
        public string quitMessage;
    }
        
    public class ExamOption
    {        
        public string tableName;
        public string courseName;
        public string columnName;
        public string description;
        public ExamItemGenerateType genType;
        public ExamItemAnswerDisplayType dispType;
        public int count = 5;
        public int askbackTimeLimitation = 1;
        public QuitType quitType;
        public string quitMessage;
        public string summerizerClassName;
        public string restartMessage;
    }

    public class ExamSelectionContext: ISelectionContext
    {
        public int askbackTime = 0;
    }
}
