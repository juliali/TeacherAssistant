using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.Exam
{
    public class ExamInfoStore
    {
        private static ExamInfoStore instance;

        private static Dictionary<string, ExamInfo> examInfoMap;

        private ExamInfoStore()
        {
            examInfoMap = new Dictionary<string, ExamInfo>();
        }

        public static ExamInfoStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExamInfoStore();
                }
                return instance;
            }
        }

        public ExamInfo GetExamInfo(string userId)
        {
            // If userId is null, set a uniq id for each uatterance.
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("Error: User id is invalid.");
            }

            if (examInfoMap.ContainsKey(userId))
            {
                return examInfoMap[userId];
            }
            else
            {                
                return null;
            }
        }

        public void SetExamInfo(string userId, ExamInfo info)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("Error: User id is invalid.");
            }

            if (examInfoMap.ContainsKey(userId))
            {
                examInfoMap[userId] = info;
            }
            else
            {
                examInfoMap.Add(userId, info);
            }
        }

        public void RemoveExamInfo(string userId)
        {
            if (examInfoMap.ContainsKey(userId))
            {
                examInfoMap.Remove(userId);
            }
        }
    }
}
