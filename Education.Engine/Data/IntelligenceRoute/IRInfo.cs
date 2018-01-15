using Education.Engine.Data.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.IntelligenceRoute
{    

    public class IRItem
    {
        public int index;
        public int stepNo;    
        public string content;
        public string options;    
        public string optionRegex;
        public string nextStep;
        public string courseName;
        public string askbackMessage;
        
    }
    public class IRInfo
    {
        public string courseName;
        public string description;
        public int askBackLimitTimes = 1;
        public Dictionary<int,IRItem> items = new Dictionary<int, IRItem>();
               
        public string quitMessage;
        public string restartMessage;      
    }

    public class IRProcessContext: ISelectionContext
    {
        public int askbackTime = 0;
        public int currentStepNo = 1;
        public int nextStepNo = 1;
        public List<int> preStepNumbers = new List<int>();
    }
}
