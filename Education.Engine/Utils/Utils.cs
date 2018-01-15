using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Education.Engine.Utils
{
    public class Utils
    {
        private static string GenerateSeparator(int len)
        {
            string sep = "";
            for(int i = 0; i < len; i ++)
            {
                sep += " ";
            }
            return sep;
        }
        public static string GenerateTable(int tableSize)
        {
            string tableStr = "\r\n";
            //string separator = "\t";
            int separatorLength = 4;

            for (int i = 0; i <= tableSize; i++)
            {
                string line = "";
                string item1 = "";
                if (i == 0)
                {
                    item1 = "x";
                }
                else
                {
                    item1 = i.ToString();
                }

                line += item1;
                int itemLen = item1.ToArray<char>().Length;
                for (int j = 1; j <= tableSize; j++)
                {
                    line += GenerateSeparator(separatorLength - itemLen);

                    string item = "";
                    if (i == 0)
                    {
                        item = j.ToString();
                    }
                    else
                    {
                        item = (j * i).ToString();
                    }
                    line += item;

                    itemLen = item.ToArray<char>().Length;
                }

                line += "\r\n";

                tableStr += line;
            }

            return tableStr;
        }

        public static string ReadEmbeddedResourceFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        public static bool IsRegexMatched(string rule, string input)
        {
            if (string.IsNullOrWhiteSpace(rule))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            Regex regex = new Regex(rule);
            Match match = regex.Match(input);

            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
