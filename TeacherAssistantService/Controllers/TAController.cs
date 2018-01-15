using Education.Engine.Main;
using log4net;
using Microsoft.EntityTypes;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Http;
using static TeacherAssistantService.Data.Response;

namespace TeacherAssistantService.Controllers
{
    public class TAController : ApiController
    {
        private static  readonly string DefaultAppId = ConfigurationManager.AppSettings["LuisAppId"];
        private static readonly string DefaultSubscriptionKey = ConfigurationManager.AppSettings["LuisSubscription"];
        private static DialogManager dm = new DialogManager(DefaultAppId, DefaultSubscriptionKey);

        private static ILog log = LogManager.GetLogger("TAController");


        [HttpPost]
        public mst_bot_response AskEducationEngine()
        {
            var jsonString = Request.Content.ReadAsStringAsync().Result;

            var botRequest = JsonConvert.DeserializeObject<mst_bot_request>(jsonString);

            dynamic q = botRequest.data["query"];
            dynamic u = botRequest.data["userid"];
            string question = q.text;
            string userId = u.text;

            log.Info("userid: " + userId + "; query: " + question);

            ConversationInfo resp = dm.Answer(userId, question);

            oaskills_textanswer answer = new oaskills_textanswer();
            answer.answer = resp.answer;
            //answer.userid = userId;
            //answer.query = question;
            answer.contextinfo = "";

            if (resp.context != null)
            {
                answer.contextinfo = JsonConvert.SerializeObject(resp.context);//resp.context.ToString();
            }
            
            var map = new mst_map();
            map.Add("answer", answer);
            return new mst_bot_response() { result = map };
        }

        [HttpGet]
        public string AskTextPlain([FromUri]string userId = "123", [FromUri]string query = null)
        {
            string resp = dm.Answer(userId, query).answer;
            return resp;
        }
       
    }
}
