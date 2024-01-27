using System;
using System.Collections;
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

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                yield return current;

                foreach (HtmlElement child in current.Children)
                    queue.Enqueue(child);
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public IEnumerable<HtmlElement> FindElements(Selector selector)
        {
            List<HtmlElement> result = new List<HtmlElement>();
            FindElementsRecursively(this, selector, result);
            return result;
        }

        private void FindElementsRecursively(HtmlElement element, Selector selector, List<HtmlElement> result)
        {
            if (!element.IsMatch(selector))
                return;

            result.Add(element);

            if (selector.Child != null)
                foreach (var child in element.Descendants())
                    FindElementsRecursively(child, selector.Child, result);

            /*if ((query.TagName == null || descendant.Name == query.TagName)
                && (query.Id == null || query.Id == descendant.Id)
                && (query.Classes.All(c => descendant.Classes.Any(cr => c == cr))))
                {
                    if (query.Child == null)
                        elementsMatched.Add(descendant);
                    FindMatchElement(descendant.Descendants(), query.Child, elementsMatched);*/
        }

        private bool IsMatch(Selector selector)
        {
            throw new NotImplementedException();
        }
    }
}
