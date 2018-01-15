using Education.Engine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.Exam
{
    


    public class ExamContextStore
    {
        private static ExamContextStore instance;

        private static Dictionary<string, ExamContext> contextMap;

        private ExamContextStore()
        {
            contextMap = new Dictionary<string, ExamContext>();
        }

        public static ExamContextStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExamContextStore();
                }
                return instance;
            }
        }

        public ExamContext GetContext(string userId)
        {
            // If userId is null, set a uniq id for each uatterance.
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("Error: User id is invalid.");
            }

            if (contextMap.ContainsKey(userId))
            {
                return contextMap[userId];
            }
            else
            {
                return null;
            }
        }

        public void SetContext(string userId, ExamContext context)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("Error: User id is invalid.");
            }

            if (contextMap.ContainsKey(userId))
            {
                contextMap[userId] = context;
            }
            else
            {
                contextMap.Add(userId, context);
            }
        }

        public void RemoveContext(string userId)
        {
            if (contextMap.ContainsKey(userId))
            {
                contextMap.Remove(userId);
            }
        }
    }
}
