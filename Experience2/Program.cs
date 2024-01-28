using Experience2;
using System.Text.RegularExpressions;


var html = await Load("https://mail.google.com/mail/u/0/#inbox");
html = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
var htmlLines = new Regex("<(.*?)>").Split(html).Where(x => x.Length > 0).ToArray();

HtmlElement root = CreateChild(htmlLines[1].Split(' ')[0], null, htmlLines[1]);
ParseHtml(root, htmlLines.Skip(2).ToList());

Console.WriteLine("HTML Tree:");
PrintHtmlTree(root, "");


var list = root.FindElements(Selector.ParseSelectorString("form div div.YhhY8"));


async Task<string> Load(string url) => await (await (new HttpClient()).GetAsync(url)).Content.ReadAsStringAsync();


static HtmlElement ParseHtml(HtmlElement rootElement, List<string> htmlLines)
{
    HtmlElement currentParent = rootElement;

    foreach (var line in htmlLines)
    {
        if (line.StartsWith("/html"))
            break;

        if (line.StartsWith("/"))
        {
            currentParent = currentParent.Parent;
            continue;
        }

        string tagName = line.Split(' ')[0];
        if (!HtmlHelper.Instance.Tags.Contains(tagName))
        {
            currentParent.InnerHtml += line;
            continue;
        }

        HtmlElement child = CreateChild(tagName, currentParent, line);
        currentParent.Children.Add(child);

        if (!HtmlHelper.Instance.TagsNoClosing.Contains(tagName) && !line.EndsWith("/"))
            currentParent = child;
    }
    return rootElement;
}


static HtmlElement CreateChild(string tagName, HtmlElement currentParent, string line)
{
    HtmlElement child = new HtmlElement { Name = tagName, Parent = currentParent };

    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    foreach (var attr in attributes)
    {
        string attributeName = attr.ToString().Split('=')[0];
        string attributeValue = attr.ToString().Split('=')[1].Replace("\"", "");

        if (attributeName.ToLower() == "class")
            child.Classes.AddRange(attributeValue.Split(' '));
        else if (attributeName.ToLower() == "id")
            child.Id = attributeValue;
        else child.Attributes.Add(attributeName, attributeValue);
    }
    return child;
}


static void PrintHtmlTree(HtmlElement element, string indentation)
{
    Console.WriteLine($"{indentation}{element}");
    foreach (var child in element.Children)
        PrintHtmlTree(child, indentation + "  ");
}
