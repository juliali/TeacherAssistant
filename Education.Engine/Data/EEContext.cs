using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data
{
    
    public abstract class EEContext
    {
        public string intent;
        public string userId;
        public DateTime timestamp;
        public StageType stage;

        public EEContext(string userId, string intent)
        {
            this.intent = intent;
            this.userId = userId;
            this.timestamp = DateTime.Now;
        }

       
        override
        public string ToString()
        {
            return "intent: " + intent +"; userid: " + userId + "; timestamp:" + timestamp.ToString("yyyy-MM-dd hh:mm:ss.SSS") ;
        }
    }

    public interface ISelectionContext
    {
    }

    public enum StageType
    {
        Init, Start, InProcess, Paused, Restarted, Completed
    }
}
