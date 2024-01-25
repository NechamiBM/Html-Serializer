using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Experience2
{
    internal class HtmlHelper
    {
        private static readonly HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public List<string> Tags { get; set; }
        public List<string> TagsNoClosing { get; set; }
        private HtmlHelper()
        {
            string jsonText = File.ReadAllText("seed/HtmlTags.json");
            Tags = JsonSerializer.Deserialize<List<string>>(jsonText);
            
            jsonText = File.ReadAllText("seed/HtmlVoidTags.json");
            TagsNoClosing = JsonSerializer.Deserialize<List<string>>(jsonText);
        }
    }
}
