using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHandler.Utils
{
    public class MDItemGenerator
    {
        public static void GenerateMultiplication(string filepath)
        {
            List<string> items = new List<string>();
            for (int i = 1; i <=9; i ++)
            {
                for (int j = 1; j <= 9; j ++)
                {
                    string item = i.ToString() + " x " + j.ToString() + "=";
                    item += "\t" + (i * j).ToString() + "\t\tInt\t^[0-9]+$\t1\t乘法\t20\tAnswer only accepts numbers. Please input answer again, or input any charactor to quit.";
                    items.Add(item);
                    
                }
            }
            File.AppendAllLines(filepath, items);
        }
        public static void GenerateDivid(string filepath)
        {
            List<string> items = new List<string>();
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    string item = (i * j).ToString() + " ÷ " + j.ToString() + "=";
                    item += "\t" + i.ToString() + "\t\tInt\t^[0-9]+$\t1\t除法\t20\tAnswer only accepts numbers. Please input answer again, or input any charactor to quit.";
                    items.Add(item);
                    
                }
            }
            File.AppendAllLines(filepath, items);
        }

    }
}
