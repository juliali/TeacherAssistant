using System;
using System.Collections.Generic;
using DoradoBot.Common.Data;
using DoradoBot.Common.LUEngine.Luis;
using DoradoBot.Common.LUEngines.RulebasedEngine;
using DoradoBot.Common.Utils;

namespace DoradoBot.Common.LUEngines
{
    public class LUController
    {
        private LuisClient luisClient;// = new LuisClient();

        private static RuleTextStore ruleStore = RuleTextStore.Instance;
     
        public LUController(string appId, string subKey)
        {
            this.luisClient = new LuisClient(appId, subKey);
        }

        public LUInfo Understand(string utterance)
        {
            DateTime startTime = DateTime.Now;
            string newUtterance = ruleStore.Preprocess(utterance);

            LUInfo info = this.luisClient.Query(newUtterance); 

            string ruleBasedIntent = ruleStore.DetermineIntent(utterance);

            if (!string.IsNullOrWhiteSpace(ruleBasedIntent))
            {
                info.Intent.intent = ruleBasedIntent;
                info.Intent.score = 1;
            }

            List<Entity> rulebasedEntities = ruleStore.ExtractSlot(utterance);              
                   
            if (rulebasedEntities.Count > 0)
            {
                info.EntityList.AddRange(rulebasedEntities);
            }

            DateTime endTime = DateTime.Now;

            LogUtils.Log(startTime, endTime, "[LU Service]");
            return info;
        }
    }
}
