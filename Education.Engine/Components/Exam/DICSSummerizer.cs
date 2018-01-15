using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.Engine.Data;
using Newtonsoft.Json;
using Education.Engine.Data.Exam;

namespace Education.Engine.Components.Exam
{
    class DICSReviewInfo
    {
        public string Name;
        public string Description;
    }

    public class DICSSummerizer : IExamSummerizer
    {
        public string GetSummary(ExamProcessContext processContext, ExamInfo examInfo)
        {
            string reviewContext = Utils.Utils.ReadEmbeddedResourceFile("Education.Engine.Res.dicsreviews.json");
            Dictionary<string, DICSReviewInfo> reviewDict = JsonConvert.DeserializeObject<Dictionary<string, DICSReviewInfo>>(reviewContext);

            Dictionary<string, int> examResults = new Dictionary<string, int>();

            foreach(string key in reviewDict.Keys)
            {
                examResults.Add(key, 0);
            }

            int index = 0;
            foreach(string result in processContext.collectedUserAnswer)
            {
                Dictionary<string, string> answerDict = GetAnswerDict(examInfo.items[index].answer);
                if (answerDict != null)
                {
                    string realResult = answerDict[result.Trim()];

                    examResults[realResult] += 1;
                }
            }

            int averageNum = examInfo.items.Count / examResults.Count;

            string resp = "你的答案是：";

            string subResp = "";
            int featureNum = 0;
            string featureStr = "";

            foreach(string key in examResults.Keys)
            {
                int num = examResults[key];

                resp += key + " " + num.ToString() + "; ";

                if (num > averageNum)
                {
                    featureNum++;
                    featureStr += key + ",";
                    subResp += reviewDict[key].Name + "\r\n" + reviewDict[key].Description + "\r\n";
                }
            }

            resp += "\r\n你的答案中有" + featureNum.ToString() + "项得分超过" + averageNum.ToString() + "。所以你具备" + featureNum.ToString() + "项特征： \r\n" + subResp;

            return resp;
        }

        private Dictionary<string, string> GetAnswerDict(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
            {
                return null;
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();

            string[] tmps = answer.Split(';');
            foreach(string tmp in tmps)
            {
                string[] newTmps = tmp.Trim().Split('-');
                if (newTmps.Length == 2)
                {
                    dict.Add(newTmps[0].Trim(), newTmps[1].Trim());
                }
            }
            return dict;
        }
    }
}
