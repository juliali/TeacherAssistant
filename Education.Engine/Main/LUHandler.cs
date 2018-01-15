using DoradoBot.Common.Data;
using DoradoBot.Common.LUEngine.Luis;
using DoradoBot.Common.LUEngines;
using Education.Engine.Data;
using Education.Engine.Data.Exam;
using Education.Engine.Data.IntelligenceRoute;
using Education.Engine.Data.QA;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Education.Engine.Main
{
    public class LUHandler
    {
        private static RuleTextStore ruleStore = RuleTextStore.Instance;

        private LuisClient luisClient;// = new LuisClient();
        private static ContextStore cStore = ContextStore.Instance;
        private static IRContextStore irCStore = IRContextStore.Instance;
        private static ExamContextStore examCStore = ExamContextStore.Instance;

        private static ILog log = LogManager.GetLogger("LUHandler");

        public LUHandler(string appId, string subKey)
        {
            this.luisClient = new LuisClient(appId, subKey);
        }

        public EEContext Understand(string userId, string userInput)
        {           
            EEContext context = cStore.GetContext(userId);

            if ((context == null) || (!IsValid(context)))
            {
                log.Info("No valid EEContxt exists. Create new EEContext.");
                LUInfo luinfo = this.Parse(userInput);

                context = CreateContext(userId, luinfo);
                cStore.SetContext(userId, context);
            }
            else
            {
                log.Info("EEContxt exists. \r\n" + JsonConvert.SerializeObject(context));
                MergeContext(userId, userInput, ref context);
                cStore.SetContext(userId, context);
            }

            return context;
        }

        public void Refresh(string userId, EEContext context)
        {
            if (context.stage == StageType.Completed || context.stage == StageType.Paused)
            {
                cStore.RemoveContext(userId);
            }
            else
            { 
                cStore.SetContext(userId, context);
            }
        }


        private LUInfo Parse(string utterance)
        {
            LUInfo currentLUInfo = null;

            if (string.IsNullOrWhiteSpace(utterance))
            {
                currentLUInfo = new LUInfo();
                currentLUInfo.Intent = new Intent();
                currentLUInfo.Intent.intent = "Greeting";
                currentLUInfo.EntityList = new List<Entity>();
            }
            else
            {
                utterance = ruleStore.Preprocess(utterance);

                string ruleBasedIntent = ruleStore.DetermineIntent(utterance);
                
                if (!string.IsNullOrWhiteSpace(ruleBasedIntent))
                {
                    currentLUInfo = new LUInfo();
                    currentLUInfo.Intent = new Intent();
                    currentLUInfo.Intent.intent = ruleBasedIntent;
                    currentLUInfo.Intent.score = 1;
                    currentLUInfo.EntityList = new List<Entity>();
                }

                if (currentLUInfo == null)
                {
                    currentLUInfo = this.luisClient.Query(utterance);
                }

                List<Entity> rulebasedEntities = ruleStore.ExtractSlot(utterance);
                if (rulebasedEntities.Count > 0)
                {
                    currentLUInfo.EntityList.AddRange(rulebasedEntities);
                }
            }

            return currentLUInfo;
        }


        private bool IsValid(EEContext context)
        {            
            DateTime currentTime = DateTime.Now;
            if (currentTime.Subtract(context.timestamp).Hours >= 8)
            {
                log.Info("[Warn]: EEContext is invalid.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private EEContext CreateContext(string userId, LUInfo luInfo)
        {
            EEContext context;
            string intent = luInfo.Intent.intent;
            if (intent == "DoExam")
            {
                context = examCStore.GetContext(userId);
                if (context == null)
                { 
                    context = new ExamContext(userId);
                }
            }
            else if (intent == "IntelligenceRoute")
            {
                context = irCStore.GetContext(userId);
                if (context == null)
                {
                    context = new IRContext(userId);
                }
            }
            else
            {
                context = new QAContext(userId, intent, luInfo.EntityList);
            }

            return context;
        }

        private void MergeContext(string userId, string userInput, ref EEContext context)
        {
            if (context.intent == "DoExam")
            {
                return;
            }
            else if(context.intent == "IntelligenceRoute")
            {
                return;
            }
            else
            {
                LUInfo luinfo = this.Parse(userInput);
                context.intent = luinfo.Intent.intent;

                if (luinfo.EntityList != null && luinfo.EntityList.Count > 0)
                { 
                    ((QAContext)context).entities = luinfo.EntityList;
                }

                if (context.intent == "DoExam")
                {
                    ExamContext savedContext = examCStore.GetContext(userId);
                    if (savedContext != null && IsValid(savedContext))
                    {
                        context = savedContext;
                    }
                }
                else if (context.intent == "IntelligenceRoute")
                {
                    IRContext savedContext = irCStore.GetContext(userId);
                    if (savedContext != null && IsValid(savedContext))
                    {
                        context = savedContext;
                    }
                }
            }            
        }
    }
}
