using Education.Engine.Controllers;
using Education.Engine.Data;
using Education.Engine.Data.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Components.Exam
{
    public class ExamProcessor
    {        
        public ExamProcessor()
        {

        }

        private string GetReviewImmediately(string userAnswer, string currentCorrectAnswer, string currentInterpret)
        {            
            string respStr = "";
            if (!string.IsNullOrWhiteSpace(currentCorrectAnswer))
            {
                if (!string.IsNullOrWhiteSpace(currentCorrectAnswer))
                {
                    respStr += "你答";
                    if (currentCorrectAnswer.ToLower().Trim() == userAnswer.ToLower().Trim())
                    {
                        respStr += "对啦！\r\n";
                    }
                    else
                    {
                        respStr += "错了。正确答案是：" + currentCorrectAnswer + "。\r\n";
                    }

                    if (!string.IsNullOrWhiteSpace(currentInterpret))
                    {
                        respStr += currentInterpret + "\r\n";
                    }
                }
            }
            return respStr;
        }

        public string ProcessItem(string userId, string userAnswer, ref ExamContext context, ref ExamInfo examInfo)
        {                        
            int index =  context.processContext.index;
            context.stage = StageType.InProcess;
            
            ExamItem item = examInfo.items[index];

            if (Utils.Utils.IsRegexMatched(item.answerRegex, userAnswer))
            {
                context.processContext.index++;
                context.processContext.askbackTime = 0;
                context.processContext.collectedUserAnswer.Add(userAnswer);                

                if (context.processContext.index == examInfo.items.Count) // Finished
                {
                    string respStr = "";
                    if (examInfo.dispType == ExamItemAnswerDisplayType.OnebyOne)
                    {
                        respStr += GetReviewImmediately(userAnswer, item.answer, item.interpret) + "\r\n";
                    }
                    respStr += "\r\n" + GetSummary(context.processContext, examInfo);
                    context.stage = StageType.Completed;
                    return respStr;
                }
                else
                {
                    string nextQuestion = examInfo.items[context.processContext.index].question;
                    string respStr = "";
                    if (examInfo.dispType == ExamItemAnswerDisplayType.AllInSummary)
                    {
                        respStr += nextQuestion;
                    }
                    else if (examInfo.dispType == ExamItemAnswerDisplayType.OnebyOne)
                    {                                                                      
                        respStr += GetReviewImmediately(userAnswer, item.answer, item.interpret) + "\r\n";
                        respStr += nextQuestion;
                    }
                    return respStr;
                }
            }
            else
            {
                context.processContext.askbackTime++;

                if (context.processContext.askbackTime > examInfo.askBackLimitTimes)
                {
                    string respStr = "";
                    switch (examInfo.quitType)
                    {
                        case QuitType.CachedWithoutSummry:                            
                            respStr += examInfo.quitMessage;
                            context.stage = StageType.Paused;
                            break;
                        case QuitType.DispatchedWithoutSummry:                            
                            respStr += examInfo.quitMessage;
                            context.stage = StageType.Completed;
                            break;
                        case QuitType.SummeryImmediately:
                            respStr += examInfo.quitMessage + "\r\n";
                            respStr += GetSummary(context.processContext, examInfo);
                            context.stage = StageType.Completed;
                            break;
                    }

                    return respStr;
                }
                else
                {                    
                    return item.askbackMessage + "\r\n" + item.question;
                }
            }
        }

  

        public string GetSummary(ExamProcessContext context, ExamInfo examInfo)
        {
            if (string.IsNullOrWhiteSpace(examInfo.summerizerClassName))
            {
                return "";
            }

            string className = examInfo.summerizerClassName;
            Type type = Type.GetType(examInfo.summerizerClassName);            
            Object obj = Activator.CreateInstance(type);
            MethodInfo methodInfo = type.GetMethod("GetSummary");
            
            object[] parametersArray = new object[] { context, examInfo };

            string respStr = (string) methodInfo.Invoke(obj, parametersArray);

            return respStr;            
        }
    }
}
