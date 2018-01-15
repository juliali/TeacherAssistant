using Education.Engine.Data.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.IntelligenceRoute
{
    
    public class IRSelectionInfo
    {
        public string optionMessage;        
        public int askbackTimeLimitation = 1;                
        public string optionRegex;
        public string askbackMessage;
        public string quitMessage;
    }
        

    public class IROption
    {        
        public string tableName;
        public string courseName;
        public string description;
        public int askbackTimeLimitation = 1;        
        public string quitMessage;
        public string restartMessage;
    }

    public class IRSelectionContext: ISelectionContext
    {
        public int askbackTime = 0;
    }
}
