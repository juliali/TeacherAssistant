using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data
{
    public class ContextStore
    {
          private static ContextStore instance;

            private static Dictionary<string, EEContext> contextMap;

            private ContextStore()
            {
                contextMap = new Dictionary<string, EEContext>();
            }

            public static ContextStore Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new ContextStore();
                    }
                    return instance;
                }
            }

            public EEContext GetContext(string userId)
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

            public void SetContext(string userId, EEContext context)
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
