﻿using Education.Engine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data.IntelligenceRoute
{
    

    public class IRContextStore
    {
        private static IRContextStore instance;

        private static Dictionary<string, IRContext> contextMap;

        private IRContextStore()
        {
            contextMap = new Dictionary<string, IRContext>();
        }

        public static IRContextStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IRContextStore();
                }
                return instance;
            }
        }

        public IRContext GetContext(string userId)
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

        public void SetContext(string userId, IRContext context)
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
