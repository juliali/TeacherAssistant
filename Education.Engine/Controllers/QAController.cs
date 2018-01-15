using Education.Engine.Components.QA;
using Education.Engine.Data;
using Education.Engine.Data.QA;

namespace Education.Engine.Controllers
{
    public class QAController
    {
        private IntentMapper mapper = new IntentMapper();

        public QAController()
        {
            
        }
        public string HandleMessage(string userId, string userInput, ref EEContext eContext)
        {
            QAContext context = (QAContext)eContext;
           
            string answer = this.mapper.GetResponse(userId, userInput, context);

            return answer;
        }
    }
}
