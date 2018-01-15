using Education.Engine.Controllers;
using Education.Engine.Data.Exam;
using Education.Engine.Main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Education.Engine
{
    class DICS
    {
        public int QuestionSeqNo;
        public DICSOption[] options;
    }

    class DICSOption
    {
        public int OptionSeqNo;
        public string OptionContent;
        public string OptionFeature;
    }

    public class Program
    {

        public static void ReadObj()
        {
            string mapFile = "Education.Engine.Res.examoptionmap.json";

            string mapContent = Utils.Utils.ReadEmbeddedResourceFile(mapFile);

            Dictionary<string, ExamOption> options = JsonConvert.DeserializeObject<Dictionary<string, ExamOption>>(mapContent);

            Console.WriteLine(options.ToString());
        }

        public static void ReadDICS()
        {
            string path = "Education.Engine.Res.tt.json";
            string content = Utils.Utils.ReadEmbeddedResourceFile(path);

            DICS[] dics = JsonConvert.DeserializeObject<DICS[]>(content);

            string[] lines = new string[dics.Length];
            string regex = "^[1-4]{1}$";
            string askbackMsg = "请输入对应选项序号，不要有其他字符。如想退出测试，请输入任何字母或汉字。";
            int i = 0;
            foreach (DICS item in dics)
            {
                string question = "在下面几项中选择一个最符合你自己的描述\\r\\n";
                string answerMap = "";
                foreach(DICSOption option in item.options)
                {
                    question += option.OptionSeqNo + "、 " + option.OptionContent + "\\r\\n";
                    answerMap += option.OptionSeqNo + "-" + option.OptionFeature + ";";
                }
                Console.WriteLine(question);
                Console.WriteLine(answerMap);
                string line = question + "\t" + answerMap + "\t" + "\t" +"String" + "\t" + regex + "\t1\tDICS\t" + askbackMsg;
                lines[i] = line;
                i++;
            }

            System.IO.File.WriteAllLines(@"D:\\data\\ta\\dics.tsv", lines);
        }
        /*
        public static void TestExam()
        {
            string mapFile = "Education.Engine.Res.examoptionmap.json";
            string selectionFile = "Education.Engine.Res.examselection.json";
            ExamController controller = new ExamController(mapFile, selectionFile);
            string userId = "123";

            Console.Clear();

            DateTime dat = DateTime.Now;
            
            Console.WriteLine("\nToday is {0:d} at {0:T}.", dat);

            Console.WriteLine(controller.Start(userId));

            string input = Console.ReadLine();
            while( input != "quit")
            {
                if (input.ToLower().StartsWith("dotest"))
                {
                    Console.WriteLine(controller.Start(userId));                 
                }
                else
                { 
                string resp = controller.HandleMessage(userId, input);
                Console.WriteLine(resp);
                }
                input = Console.ReadLine();
            }

            Console.WriteLine("Quit!");
        }

        public static void TestIR()
        {
            string mapFile = "Education.Engine.Res.iroptionmap.json";
            string selectionFile = "Education.Engine.Res.irselection.json";
            IRController controller = new IRController(mapFile, selectionFile);
            string userId = "123";

            Console.Clear();

            DateTime dat = DateTime.Now;

            Console.WriteLine("\nToday is {0:d} at {0:T}.", dat);

            Console.WriteLine(controller.Start(userId));

            string input = Console.ReadLine();
            while (input != "quit")
            {
                if (input.ToLower().StartsWith("dotest"))
                {
                    Console.WriteLine(controller.Start(userId));
                }
                else
                {
                    string resp = controller.HandleMessage(userId, input);
                    Console.WriteLine(resp);
                }
                input = Console.ReadLine();
            }

            Console.WriteLine("Quit!");
        }
        */

        public static void TestBot()
        {
            DialogManager dm = new DialogManager("be658451-50ed-4dcc-8787-5c35d4d676e3", "3d4bd4fa6ad349b2ad07b86163d463be");
            Console.Write("[User]: ");
            string input = Console.ReadLine();

            string userId = "123";

            while (input != "quit")
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Write("\r\n[User]: ");
                    input = Console.ReadLine();
                }
                else
                { 
                string resp = dm.Answer(userId, input).answer;
                Console.WriteLine("\r\n[Bot]: " + resp);
                Console.Write("\r\n[User]: ");
                input = Console.ReadLine();
                }
            }

            Console.WriteLine("Quit!");
        }

        public static void GenerateTable(int tableSize)
        {
            string path = "d:\\data\\ta\\mt_" + tableSize.ToString() + ".txt";

            string content = Utils.Utils.GenerateTable(tableSize);            
            File.WriteAllText(path, content);
        }

        public static void Main(string[] args)
        {
            //ReadObj();
            //ReadDICS();
            //TestExam();
            //TestIR();

            TestBot();
          /*  GenerateTable(3);
            GenerateTable(5);
            GenerateTable(7);
            GenerateTable(9);
            GenerateTable(12);*/
            Console.WriteLine("Finished!");
        }
    }
}
