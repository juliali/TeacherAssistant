using DoradoBot.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.QA
{
    public class QAContext:EEContext
    {
        public QAContext(string userId, string intent, List<Entity> entities): base(userId, intent)
        {
            this.entities = entities;
            this.stage = StageType.InProcess;
        }

        public List<Entity> entities;
    }
}
