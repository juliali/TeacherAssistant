using DoradoBot.Common.Data;
using Education.Engine.Data;
using Education.Engine.Data.QA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Components.QA
{


    public interface IAnswerQuerier
    {
        string GetResponse(string userId, string intent, List<Entity> entities);        
    }
}
