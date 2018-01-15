using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoradoBot.Common.Data;
using System.Text.RegularExpressions;

namespace Education.Engine.Components.QA
{
    public class MTQuerier : IAnswerQuerier
    {
        public string GetResponse(string userId, string intent, List<Entity> entities)
        {
            string response = "";
            
            int tableSize = GetTableSize(entities);

            if (tableSize > 12)
            {
                tableSize = 12;
                response += "The largest multiplication table to show is 12x12 one.\r\n";
            }

            string tableStr = Utils.Utils.GenerateTable(tableSize);

            response += tableStr;

            return response;
        }

       /* private string GenerateTable(int tableSize)
        {
            string tableStr = "";
            string separator = "\t";
            
            for (int i = 0; i <= tableSize; i ++)
            {
                string line = "";
                if (i == 0)
                {
                    line += "x";
                }
                else
                {
                    line += i.ToString();
                }

                for(int j = 1; j <= tableSize; j ++)
                {
                    line += separator;

                    if (i == 0)
                    {
                        line += j.ToString();
                    }
                    else
                    {
                        line += (j * i).ToString();
                    }
                }
                line += "\r\n";

                tableStr += line;
            }

            return tableStr;
        }*/

        private int GetTableSize(List<Entity> entities)
        {
            int tableSize = 0;

            foreach(Entity entity in entities)
            {
                if (entity.type == "number")
                {
                    int num = int.Parse(entity.value);
                    if (num > tableSize)
                    {
                        tableSize = num;
                    }
                }
                else if (entity.type == "tablesize")
                {
                    tableSize = ParseNumberFromTableSizeEntity(entity.value);
                    break;
                }
            }

            return tableSize;
        }

        private int ParseNumberFromTableSizeEntity(string str)
        {
            string rule = "(x|X|\\*)";
            Regex regex = new Regex(rule);
            string[] tmps = regex.Split(str);
            int tableSize = 0;
            foreach (string tmp in tmps)
            {
                if (!Utils.Utils.IsRegexMatched(rule, tmp))
                { 
                int num = int.Parse(tmp);
                if (num > tableSize)
                {
                    tableSize = num;
                }
                }
            }

            return tableSize;
        }
    }
}
