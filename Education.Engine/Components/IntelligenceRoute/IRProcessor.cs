using Education.Engine.Controllers;
using Education.Engine.Data;
using Education.Engine.Data.Exam;
using Education.Engine.Data.IntelligenceRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Components.IntelligenceRoute
{
    public class IRProcessor
    {        
        public IRProcessor()
        {

        }       

        private int GetNextStep(List<int> histroy, string nextStep, string userInput)
        {            
            string[] tmps = nextStep.Trim().Split(';');
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach(string tmp in tmps)
            {
                string[] pair = tmp.Trim().Split(':');
                if (pair.Length == 2)
                {
                    string option = pair[0].Trim();
                    string step = pair[1].Trim();

                    try
                    {
                        int stepNo = int.Parse(step);
                        dict.Add(option, stepNo);
                    }
                    catch(Exception e)
                    {
                        if (step == "b")
                        {
                            int stepNo = histroy[histroy.Count - 1];
                            dict.Add(option, stepNo);
                        }
                    }
                }
            }

            int result = dict[userInput.Trim()];
            return result;
        }

        private int GetLastStep(Dictionary<int, IRItem> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new Exception("Steps are invalid.");
            }

            int[] sortStepList = items.Keys.ToArray<int>();           
            sortStepList = sortStepList.OrderByDescending(c => c).ToArray();
            return sortStepList[0];
        }

        public string ProcessItem(string userId, string userAnswer, ref IRContext context, ref IRInfo info)
        {                                    
            //context.stage = StageType.InProcess;

            int currentStep = context.processContext.currentStepNo;
            IRItem item = info.items[currentStep];

            if (Utils.Utils.IsRegexMatched(item.optionRegex, userAnswer))
            {
                context.processContext.askbackTime = 0;
                int nextStep = GetNextStep(context.processContext.preStepNumbers, item.nextStep, userAnswer);

                if (nextStep == GetLastStep(info.items)) // Finished
                {                                        
                    context.stage = StageType.Completed;                    
                }

                context.processContext.preStepNumbers.Add(currentStep);

                string respStr = info.items[nextStep].content + "\r\n" + info.items[nextStep].options;

                context.processContext.currentStepNo = nextStep;
                return respStr;
            }
            else
            {
                if (context.stage == StageType.InProcess)
                { 
                    context.processContext.askbackTime++;
                }

                if (context.processContext.askbackTime > info.askBackLimitTimes)
                {
                    string respStr = info.quitMessage;
                    context.stage = StageType.Paused;
                    return respStr;
                }
                else
                {                    
                    return item.askbackMessage + "\r\n" + item.content + item.options;
                }
            }
        }         
    }
}
