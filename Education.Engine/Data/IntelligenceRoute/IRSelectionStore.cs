using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.IntelligenceRoute
{
    class IRSelectionStore
    {
        private static IRSelectionStore instance;
        
        private static Dictionary<string, IROption> selectionOptionMap;
        private static IRSelectionInfo selectionInfo;

        private static string selectionFilePath = "Education.Engine.Res.irselection.json";
        private static string mapFilePath = "Education.Engine.Res.iroptionmap.json";

        private IRSelectionStore()
        {                                    
            string json = Utils.Utils.ReadEmbeddedResourceFile(selectionFilePath);
            selectionInfo = JsonConvert.DeserializeObject<IRSelectionInfo>(json);

            json = Utils.Utils.ReadEmbeddedResourceFile(mapFilePath);
            selectionOptionMap = JsonConvert.DeserializeObject<Dictionary<string, IROption>>(json);
        }

        public static IRSelectionStore GetInstance()
        {            
            if (instance == null)
            {
                instance = new IRSelectionStore();
            }
            return instance;            
        }

        public IRSelectionInfo GetSelectionInfo()
        {
            return selectionInfo;
        }

        public Dictionary<string, IROption> GetSelectionMap()
        {
            return selectionOptionMap;
        }           
    }
}
