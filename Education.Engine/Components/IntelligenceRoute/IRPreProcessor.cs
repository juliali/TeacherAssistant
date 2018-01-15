using DBHandler.DBAccess;
using Education.Engine.Data;
using Education.Engine.Data.IntelligenceRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Components.IntelligenceRoute
{
    public class IRPreProcessor
    {                
        private string[] targetColumns = { "StepNo","Content","Options", "OptionRegex","NextStep" , "AskbackMessage" };         

        public IRInfo GenerateProcessItems(IROption selection)
        {
            IRInfo info = new IRInfo();

            if (string.IsNullOrWhiteSpace(selection.courseName))
            {
                throw new Exception("Error: course name is not decided.");
            }

            string sqlQuery = "Select * from dbo." + selection.tableName + " Where CourseName = N'" + selection.courseName + "'";            

            info.courseName = selection.courseName;
            info.description = selection.description;            
            info.askBackLimitTimes = selection.askbackTimeLimitation;
            info.items = GenerateProcessItems(sqlQuery);            
            info.quitMessage = selection.quitMessage;
            info.restartMessage = selection.restartMessage;            
            
            return info;
        }

        private Dictionary<int,IRItem> GenerateProcessItems(string sqlquery)
        {          
            SQLServerAccessor dbAccessor = new SQLServerAccessor();

            List<Dictionary<string, string>> output = dbAccessor.TableSearch(targetColumns, sqlquery);

            Dictionary<int,IRItem> dict = new Dictionary<int, IRItem>();
            int index = 0;
            foreach (Dictionary<string, string> rec in output)
            {
                IRItem item = new IRItem();
                item.index = index++;
                item.stepNo = int.Parse(rec["StepNo"]);
                item.content = rec["Content"];
                item.options = rec["Options"];
                item.optionRegex = rec["OptionRegex"];
                item.nextStep = rec["NextStep"];
                
                item.askbackMessage = rec["AskbackMessage"];
                                
                dict.Add(item.stepNo, item);
            }

            return dict;     
        }        
    }
}
