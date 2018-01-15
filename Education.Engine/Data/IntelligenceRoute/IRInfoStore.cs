using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.IntelligenceRoute
{
    public class IRInfoStore
    {
        private static IRInfoStore instance;

        private static Dictionary<string, IRInfo> irInfoMap;

        private IRInfoStore()
        {
            irInfoMap = new Dictionary<string, IRInfo>();
        }

        public static IRInfoStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IRInfoStore();
                }
                return instance;
            }
        }

        public IRInfo GetIRInfo(string userId)
        {
            // If userId is null, set a uniq id for each uatterance.
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("Error: User id is invalid.");
            }

            if (irInfoMap.ContainsKey(userId))
            {
                return irInfoMap[userId];
            }
            else
            {                
                return null;
            }
        }

        public void SetIRInfo(string userId, IRInfo info)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("Error: User id is invalid.");
            }

            if (irInfoMap.ContainsKey(userId))
            {
                irInfoMap[userId] = info;
            }
            else
            {
                irInfoMap.Add(userId, info);
            }
        }

        public void RemoveIRInfo(string userId)
        {
            if (irInfoMap.ContainsKey(userId))
            {
                irInfoMap.Remove(userId);
            }
        }
    }
}
