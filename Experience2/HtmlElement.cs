using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Experience2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; } = "";
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public override string ToString()
        {
            return Name;
        }

    }
}
