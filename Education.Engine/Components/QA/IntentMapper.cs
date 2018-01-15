using Education.Engine.Data.QA;
using System;
using System.Reflection;


namespace Education.Engine.Components.QA
{
    public class IntentMapper
    {
        private static IntentMapStore iStore = IntentMapStore.GetInstance();

        public IntentMapper()
        {  
                      
        }

        public string GetResponse(string userId, string userInput, QAContext context)
        {
            string intent = context.intent;//luInfo.intent;

            if (string.IsNullOrWhiteSpace(intent))
            {
                return null;
            }

            QueryMapInfo info = iStore.GetMapInfo(intent.ToLower());
            if (info != null)
            {
                if (info.qaType == QAType.Direct)
                {
                    return info.answer;
                }
                else if (info.qaType == QAType.File)
                {
                    string path = info.answer;
                    string answer = Utils.Utils.ReadEmbeddedResourceFile(path);
                    return answer;
                }
                else //if (info.qaType == QAType.SQLQuery)
                {
                    if (string.IsNullOrWhiteSpace(info.answer))
                    {
                        return "";
                    }

                    string className = info.answer;
                    Type type = Type.GetType(className);
                    Object obj = Activator.CreateInstance(type);
                    MethodInfo methodInfo = type.GetMethod("GetResponse");

                    object[] parametersArray = new object[] { userId, intent, context.entities };

                    string respStr = (string)methodInfo.Invoke(obj, parametersArray);

                    return respStr;
                }
            }
            else
            {
                return iStore.GetMapInfo("none").answer;                
            }
        }
    }
}
