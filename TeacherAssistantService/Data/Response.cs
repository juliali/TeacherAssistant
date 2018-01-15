using Microsoft.EntityTypes.Atomic;
using Newtonsoft.Json;


namespace TeacherAssistantService.Data
{
    public class Response
    {
        [JsonConverter(typeof(TypeJsonConverter))]
        public class oaskills_textanswer
        {
            [JsonProperty(PropertyName = "@")]
            public const string classType = "oaskills.textanswer";

            public string answer { get; set; }

           // public string query { get; set; }

           // public string userid { get; set; }

            public string contextinfo { get; set; }
        }
    }
}