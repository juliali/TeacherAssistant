using DBHandler.DBAccess;
using Education.Engine.Data;
using Education.Engine.Data.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Components.Exam
{
    public class ExamPreProcessor
    {                
        private string[] targetColumns = { "Question", "Answer", "AnswerType", "Interpret", "AnswerRegex", "Score" , "AskbackMessage" };         

        public ExamInfo GenerateProcessItems(ExamOption selection)
        {
            ExamInfo exameInfo = new ExamInfo();

            if (string.IsNullOrWhiteSpace(selection.columnName))
            {
                throw new Exception("Error: course name is not decided.");
            }

            string sqlQuery = "Select * from dbo." + selection.tableName + " Where CourseName = N'" + selection.columnName + "'";

            if (selection.genType == ExamItemGenerateType.RandomSelection)
            {
                sqlQuery = "SELECT TOP " + selection.count.ToString() + " * FROM dbo." + selection.tableName + " Where CourseName = N'" + selection.columnName + "' ORDER BY NEWID()";
            }

            exameInfo.courseName = selection.courseName;
            exameInfo.columnName = selection.columnName;
            exameInfo.description = selection.description;
            exameInfo.dispType = selection.dispType;
            exameInfo.quitType = selection.quitType;
            exameInfo.askBackLimitTimes = selection.askbackTimeLimitation;
            exameInfo.items = GenerateProcessItems(sqlQuery);
            exameInfo.summerizerClassName = selection.summerizerClassName;
            exameInfo.quitMessage = selection.quitMessage;
            exameInfo.restartMessage = selection.restartMessage;            
            
            return exameInfo;
        }

        private List<ExamItem> GenerateProcessItems(string sqlquery)
        {          
            SQLServerAccessor dbAccessor = new SQLServerAccessor();

            List<Dictionary<string, string>> output = dbAccessor.TableSearch(targetColumns, sqlquery);

            List<ExamItem> list = new List<ExamItem>();
            int index = 0;
            foreach (Dictionary<string, string> rec in output)
            {
                ExamItem item = new ExamItem();
                item.index = index++;
                item.question = rec["Question"];
                item.answerRegex = rec["AnswerRegex"];
                item.answer = rec["Answer"];
                item.answerType = rec["AnswerType"];
                item.interpret = rec["Interpret"];
                item.askbackMessage = rec["AskbackMessage"];
                if (!string.IsNullOrWhiteSpace(rec["Score"]))
                { 
                    item.score = int.Parse(rec["Score"]);
                }                
                list.Add(item);
            }

            return list;     
        }        
    }
}
