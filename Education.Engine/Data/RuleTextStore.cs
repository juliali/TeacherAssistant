using DoradoBot.Common.Data;
using DoradoBot.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Engine.Data
{
    public class RuleTextStore
    {
        private static RuleTextStore instance;

        private static List<ExtractorRuleInfo> slotRules;
        private static List<ExtractorRuleInfo> intentRules;
        private static List<PreprocessRuleInfo> preprocessRules;

        private RuleTextStore()
        {
            slotRules = this.ReadExtractorInfo("Education.Engine.Res.EntityRules.txt");
            intentRules = this.ReadExtractorInfo("Education.Engine.Res.IntentRules.txt");
            preprocessRules = this.ReadPreprocessInfo("Education.Engine.Res.ETLRules.txt");
        }

        public static RuleTextStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RuleTextStore();
                }
                return instance;
            }
        }

        public List<Entity> ExtractSlot(string utterance)
        {
            List<Entity> entities = new List<Entity>();

            foreach (ExtractorRuleInfo Rule in slotRules)
            {
                List<string> matchedValues = FileUtils.Match(Rule.ValueRegex, utterance);

                if (matchedValues.Count > 0)
                {
                    foreach (string matchedValue in matchedValues)
                    {
                        Entity entity = new Entity();
                        entity.value = matchedValue;
                        entity.type = Rule.Type;

                        if (!string.IsNullOrWhiteSpace(Rule.ResolutionValue))
                        {
                            entity.resolution = Rule.ResolutionValue;
                        }

                        entities.Add(entity);
                    }
                }
            }

            return entities;
        }

        public string DetermineIntent(string utterance)
        {
            foreach (ExtractorRuleInfo Rule in intentRules)
            {
                List<string> matchedValues = FileUtils.Match(Rule.ValueRegex, utterance);

                if (matchedValues.Count > 0)
                {
                    return Rule.Type;
                }
            }

            return null;
        }

        public string Preprocess(string utterance)
        {
            string newUtterance = utterance;

            if (string.IsNullOrWhiteSpace(utterance))
            {
                return newUtterance;
            }

            foreach (PreprocessRuleInfo info in preprocessRules)
            {
                newUtterance = FileUtils.ReplaceRegx(info, newUtterance);
            }

            return newUtterance;
        }
    

    private List<PreprocessRuleInfo> ReadPreprocessInfo(string ResourceName)
    {
        List<PreprocessRuleInfo> results = new List<PreprocessRuleInfo>();

        string content = Utils.Utils.ReadEmbeddedResourceFile(ResourceName);
        string[] lines = content.Split("\r\n".ToCharArray());

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            string[] tmps = line.Split('\t');

            if (tmps.Length == 3)
            {
                PreprocessRuleInfo info = new PreprocessRuleInfo();
                info.ValueRegex = tmps[0];
                info.OrigValue = tmps[1];
                info.NewValue = tmps[2];

                results.Add(info);
            }
        }

        return results;
    }

    private List<ExtractorRuleInfo> ReadExtractorInfo(string ResourceName)
    {
        List<ExtractorRuleInfo> results = new List<ExtractorRuleInfo>();

        string content = Utils.Utils.ReadEmbeddedResourceFile(ResourceName);
        string[] lines = content.Split("\r\n".ToCharArray());

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            string[] tmps = line.Split('\t');

            if (tmps.Length >= 2)
            {
                ExtractorRuleInfo info = new ExtractorRuleInfo();
                info.ValueRegex = tmps[0];
                info.Type = tmps[1];

                if (tmps.Length > 2)
                {
                    info.ResolutionValue = tmps[2];
                }

                results.Add(info);
            }
        }

        return results;
    }
}

}
