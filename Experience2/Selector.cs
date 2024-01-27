using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Experience2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector ParseSelectorString(string selectorString)
        {
            Selector rootSelector = new Selector();
            Selector currentSelector = rootSelector;

            string[] parts = selectorString.Split(' ');
            foreach (string part in parts)
            {
                string[] selectors = new Regex("(?=[#\\.])").Split(part).Where(s => s.Length > 0).ToArray();
                foreach (string selector in selectors)
                {
                    if (selector.StartsWith("#"))
                        currentSelector.Id = selector.Substring(1);
                    else if (selector.StartsWith("."))
                        currentSelector.Classes.Add(selector.Substring(1));
                    else if (HtmlHelper.Instance.Tags.Contains(selector))
                        currentSelector.TagName = selector;
                    else
                        throw new ArgumentException($"Invalid HTML tag name: {selector}");
                }

                Selector newSelector = new Selector();
                currentSelector.Child = newSelector;
                newSelector.Parent = currentSelector;
                currentSelector = newSelector;
            }
            currentSelector.Parent.Child = null;

            return rootSelector;
        }

    }
}
