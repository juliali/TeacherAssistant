using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.IntelligenceRoute
{
    public class IRContext : EEContext
    {
        public IRContext(string userId): base(userId, "IntelligenceRoute")
        {
            this.selectContext = new IRSelectionContext();
            this.processContext = new IRProcessContext();
            this.stage = StageType.Init;
        }
        
        public IRSelectionContext selectContext;
        public IRProcessContext processContext;
    }
}
