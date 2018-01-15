using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.QA
{
    public class IntentMapStore
    {
        private string intentMapFilePath = "Education.Engine.Res.qamap.json";
        private static IntentMapStore instance;

        private static Dictionary<string, QueryMapInfo> intentMapDict;

        private IntentMapStore()
        {
            intentMapDict = new Dictionary<string, QueryMapInfo>();
            string content = Utils.Utils.ReadEmbeddedResourceFile(intentMapFilePath);
            List<QueryMapInfo> mapInfo = JsonConvert.DeserializeObject<List<QueryMapInfo>>(content);

            foreach (QueryMapInfo map in mapInfo)
            {
                intentMapDict.Add(map.intent.ToLower(), map);
            }
        }

        public static IntentMapStore GetInstance()
        {
            if (instance == null)
            {
                instance = new IntentMapStore();
            }
            return instance;
        }

        public QueryMapInfo GetMapInfo(string intent)
        {
            string keyword = intent;

            if (!intentMapDict.ContainsKey(keyword))
            {
                return null;
            }

            return intentMapDict[keyword];
        }        
    }
}
