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

        public override string ToString()
        {
            string s = "";
            if (Name != null) s += "Name: " + Name;
            if (Id != null) s += " Id: " + Id;
            if (Classes.Count > 0)
            {
                s += " Classes: ";
                foreach (var c in Classes)
                    s += c + " ";
            }
            return s;
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                if (this != current)
                    yield return current;

                foreach (HtmlElement child in current.Children)
                    queue.Enqueue(child);
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = Parent;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public IEnumerable<HtmlElement> FindElements(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();

            foreach (var child in Descendants())
                child.FindElementsRecursively(selector, result);
            return result;
        }

        private void FindElementsRecursively(Selector selector, HashSet<HtmlElement> result)
        {
            if (!IsMatch(selector))
                return;

            if (selector.Child == null)
                result.Add(this);
            else
                foreach (var child in Descendants())
                    child.FindElementsRecursively(selector.Child, result);
        }

        private bool IsMatch(Selector selector)
        {
            return ((selector.TagName == null || Name.Equals(selector.TagName))
                && (selector.Id == null || selector.Id.Equals(Id))
                && (selector.Classes.Intersect(Classes).Count() == selector.Classes.Count));
        }
    }
}
