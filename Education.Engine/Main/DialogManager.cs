using Education.Engine.Controllers;
using Education.Engine.Data;
using Education.Engine.Data.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Main
{
    public class ConversationInfo
    {
        public EEContext context;
        public string answer;
    }

    public class DialogManager
    {
        private LUHandler luHandler;// = new LUHandler();

        private QAController qaController = new QAController();
        private ExamController examController = new ExamController();
        private IRController irController = new IRController();

        public DialogManager(string appId, string subKey)
        {
            this.luHandler = new LUHandler(appId, subKey);
        }

        public ConversationInfo Answer(string userId, string userInput)
        {
            EEContext context = luHandler.Understand(userId, userInput);

            string answer = "No Response";

            switch (context.intent)
            {
                case "DoExam":
                    answer = examController.HandleMessage(userId, userInput, ref context);
                    
                    break;
                case "IntelligenceRoute":
                    answer = irController.HandleMessage(userId, userInput, ref context);
                    break;
                default:
                    answer = qaController.HandleMessage(userId, userInput, ref context);
                    break;
            }

            if (context.stage != StageType.InProcess)
            {
                luHandler.Refresh(userId, context);
            }

            ConversationInfo result = new ConversationInfo();
            result.answer = answer;
            result.context = context;

            return result;
        }
    }
}
