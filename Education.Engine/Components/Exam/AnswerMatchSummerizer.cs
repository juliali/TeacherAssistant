using Education.Engine.Data.Exam;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Education.Engine.Components.Exam
{
    public class AnswerMatchSummerizer : IExamSummerizer
    {
        //private Dictionary<string, SummaryInfo> summaryDict;
        private SummaryInfo info;

        public AnswerMatchSummerizer()
        {
            string content = Utils.Utils.ReadEmbeddedResourceFile("Education.Engine.Res.examanswers.json");
            Dictionary<string, SummaryInfo> summaryDict = JsonConvert.DeserializeObject<Dictionary<string, SummaryInfo>>(content);

            foreach(string key in summaryDict.Keys)
            {
                if (key == this.GetType().FullName)
                {
                    this.info = summaryDict[key];
                }
            }
        }

        public string GetSummary(ExamProcessContext processContext, ExamInfo examInfo)
        {
            int currentIndex = processContext.index;

            int correctNumber = 0;
            int totalNumber = 0;
            int totalScore = 0;
            int userScore = 0;

            string error = "";

            for (int i = 0; i < currentIndex; i++)
            {
                string userAnswer = processContext.collectedUserAnswer[i];
                string correctAnswer = examInfo.items[i].answer;

                int score = examInfo.items[i].score;

                if (userAnswer.Trim().ToLower() == correctAnswer.Trim().ToLower())
                {
                    correctNumber++;
                    userScore += score;
                }
                else
                {
                    string[] tmps = correctAnswer.Trim().ToLower().Split(';');
                    if (tmps.Length > 1)
                    {
                        Array.Sort(tmps);
                        string correctMultipleAnswer = string.Join("", tmps);
                        tmps = userAnswer.Trim().ToLower().Split(' ');
                        Array.Sort(tmps);
                        string userMultipleAnswer = string.Join("", tmps);
                        if (userMultipleAnswer == correctMultipleAnswer)
                        {
                            correctNumber++;
                            userScore += score;
                            break;
                        }
                    }
                    error += this.info.SingleItemTemplate.Replace("${QUESTION}", examInfo.items[i].question).Replace("${USER_ANSWER}", userAnswer).Replace("${CORRECT_ANSWER}", correctAnswer);
                    //"Questions: " + examInfo.items[i].question + ". Answers: " + userAnswer + ". [X]  " + "Correct answers " + correctAnswer + "\r\n";
                }
                totalNumber++;
                totalScore += score;
            }

            string result = this.info.SummeryTemplate.Replace("${TOTAL_NUMBER}", totalNumber.ToString()).Replace("${CORRECT_NUMBER}", correctNumber.ToString()).Replace("${USER_SCORE}", userScore.ToString()).Replace("${TOTAL_SCORE}", totalScore.ToString());
            //"Totally " + totalNumber.ToString() + " questions, and " + correctNumber.ToString() + "correct answers, scores（" + userScore.ToString() + "/" + totalScore.ToString() + ").\r\n";

            if (examInfo.dispType == ExamItemAnswerDisplayType.AllInSummary)
            { 
                result += error;
            }

            return result;
        }
    }
}
