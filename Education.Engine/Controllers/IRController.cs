using Education.Engine.Components.Exam;
using Education.Engine.Components.IntelligenceRoute;
using Education.Engine.Data;
using Education.Engine.Data.Exam;
using Education.Engine.Data.IntelligenceRoute;
using System;
using System.Collections.Generic;

namespace Education.Engine.Controllers
{
   

   
    public class IRController
    {               
        private static IRContextStore cStore = IRContextStore.Instance;
        private static IRInfoStore eStore = IRInfoStore.Instance;
        private static IRSelectionStore sStore = IRSelectionStore.GetInstance();
        
        public IRController()
        {            
        }        

        public string HandleMessage(string userId, string userInput, ref EEContext eContext)
        {
            if (eContext.GetType() != typeof(IRContext))
            {
                eContext = new IRContext(userId);
            }
            
            IRContext context = (IRContext)eContext;                        

            IRProcessor processor = new IRProcessor();
           
            string respStr = null;
            IRSelectionInfo sInfo = sStore.GetSelectionInfo();
            Dictionary<string, IROption> selectionOptionMap = sStore.GetSelectionMap();
            IRInfo info = null;

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
                            IROption option = selectionOptionMap[userInput];
                            IRPreProcessor preProcessor = new IRPreProcessor();
                            info = preProcessor.GenerateProcessItems(option);
                            eStore.SetIRInfo(userId, info);                            

                            respStr = info.courseName + "\r\n" + (string.IsNullOrWhiteSpace(info.description)?"":( info.description+ "\r\n" )) + info.items[context.processContext.currentStepNo].content + info.items[context.processContext.currentStepNo].options;
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
                    info = eStore.GetIRInfo(userId);                    
                    respStr = processor.ProcessItem(userId, userInput, ref context, ref info);                    
                    break;
                case StageType.Paused:
                    info = eStore.GetIRInfo(userId);
                    respStr = info.restartMessage;
                    context.stage = StageType.Restarted;
                    break;
                case StageType.Restarted:
                    info = eStore.GetIRInfo(userId);
                    if (userInput.Trim().ToLower() != "y")
                    {
                        context.processContext = new IRProcessContext();

                        respStr = sInfo.optionMessage + "\r\n";
                        foreach (string key in selectionOptionMap.Keys)
                        {
                            respStr += "（" + key + "）" + selectionOptionMap[key].courseName + " ";
                        }

                        context.stage = StageType.Start;
                    }
                    else
                    { 
                        respStr = processor.ProcessItem(userId, userInput, ref context, ref info);
                        context.stage = StageType.InProcess;
                    }
                    break;
            }

            if (context.stage == StageType.Completed)
            {
                cStore.RemoveContext(userId);
                eStore.RemoveIRInfo(userId);
                eContext.intent = "none";                                
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
