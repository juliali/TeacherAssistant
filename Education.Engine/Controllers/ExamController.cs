using Education.Engine.Components.Exam;
using Education.Engine.Data;
using Education.Engine.Data.Exam;
using System;
using System.Collections.Generic;

namespace Education.Engine.Controllers
{
      
    public class ExamController
    {               
        private static ExamContextStore cStore = ExamContextStore.Instance;
        private static ExamInfoStore eStore = ExamInfoStore.Instance;
        private static ExamSelectionStore sStore = ExamSelectionStore.GetInstance();

        public ExamController()
        {        
            ExamSelectionInfo sInfo = sStore.GetSelectionInfo();
            Dictionary<string, ExamOption> selectionOptionMap = sStore.GetSelectionMap();
        }        
      
        public string HandleMessage(string userId, string userInput, ref EEContext eContext)
        {      
            if (eContext.GetType() != typeof(ExamContext))
            {
                eContext = new ExamContext(userId);
            }

            ExamContext context = (ExamContext)eContext;

            ExamProcessor processor = new ExamProcessor();
           
            string respStr = null;
            ExamSelectionInfo sInfo = sStore.GetSelectionInfo();
            Dictionary<string, ExamOption> selectionOptionMap = sStore.GetSelectionMap();
            ExamInfo info = null;

            switch (context.stage)
            {
                case StageType.Init:                    
                    respStr = sInfo.optionMessage + "\r\n";
                    foreach (string key in selectionOptionMap.Keys)
                    {
                        respStr += "（" + key + "）" + selectionOptionMap[key].courseName + " ";
                    }

                    context.stage = StageType.Start;
                    break;
                case StageType.Start:                                       
                                      
                    bool isMatched = Utils.Utils.IsRegexMatched(sInfo.optionRegex, userInput);
                    
                    if (isMatched)
                    {
                        if (selectionOptionMap.ContainsKey(userInput))
                        {
                            ExamOption option = selectionOptionMap[userInput];                            
                            ExamPreProcessor preProcessor = new ExamPreProcessor();
                            info = preProcessor.GenerateProcessItems(option);
                            eStore.SetExamInfo(userId, info);                            

                            respStr = info.courseName + "\r\n" + (string.IsNullOrWhiteSpace(info.description)?"":( info.description+ "\r\n" )) + info.items[context.processContext.index].question;
                            context.stage = StageType.InProcess;
                        }      
                        else
                        {
                            throw new Exception("Option " + userInput + " is not available.");
                        }                  
                    }
                    else
                    {                        
                        respStr = sInfo.askbackMessage + "\r\n" + sInfo.optionMessage;
                        context.selectContext.askbackTime++;
                    }
                    break;
                case StageType.InProcess:
                    info = eStore.GetExamInfo(userId);                    
                    respStr = processor.ProcessItem(userId, userInput, ref context, ref info);                    
                    break;
                case StageType.Paused:
                    info = eStore.GetExamInfo(userId);
                    respStr = info.restartMessage;
                    context.stage = StageType.Restarted;
                    break;
                case StageType.Restarted:
                    info = eStore.GetExamInfo(userId);
                    if (userInput.Trim().ToLower() != "y")
                    {
                        context.processContext = new ExamProcessContext();
                    }
                    respStr = processor.ProcessItem(userId, userInput, ref context, ref info);
                    context.stage = StageType.InProcess;
                    break;
            }

            if (context.stage == StageType.Completed)
            {
                cStore.RemoveContext(userId);
                eStore.RemoveExamInfo(userId);                                
            }
            else
            {
                if (context.stage == StageType.Paused)
                {
                    context.processContext.askbackTime = 0;
                }

                cStore.SetContext(userId, context);
            }

            return respStr;
        }

    }
}
