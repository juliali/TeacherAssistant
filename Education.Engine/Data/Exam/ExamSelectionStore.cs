using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.Exam
{
    class ExamSelectionStore
    {
        private static ExamSelectionStore instance;
        
        private static Dictionary<string, ExamOption> selectionOptionMap;
        private static ExamSelectionInfo selectionInfo;

        private static string selectionFilePath = "Education.Engine.Res.examselection.json";
        private static string mapFilePath = "Education.Engine.Res.examoptionmap.json";
        private ExamSelectionStore()
        {                                                
            string json = Utils.Utils.ReadEmbeddedResourceFile(mapFilePath);
            selectionOptionMap = JsonConvert.DeserializeObject<Dictionary<string, ExamOption>>(json);
            

            json = Utils.Utils.ReadEmbeddedResourceFile(selectionFilePath);
            selectionInfo = JsonConvert.DeserializeObject<ExamSelectionInfo>(json);            
        }

        public static ExamSelectionStore GetInstance()
        {            
            if (instance == null)
            {
                instance = new ExamSelectionStore();
            }
            return instance;            
        }

        public ExamSelectionInfo GetSelectionInfo()
        {
            return selectionInfo;
        }

        public Dictionary<string, ExamOption> GetSelectionMap()
        {
            return selectionOptionMap;
        }           
    }
}
