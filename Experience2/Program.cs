using Experience2;
using System.Text.RegularExpressions;

var html = await Load("https://learn.malkabruk.co.il/practicode/projects/pract-2/#_5");
html = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
var htmlLines = new Regex("<(.*?)>").Split(html).Where(x => x.Length > 0).ToArray();

HtmlElement root = new HtmlElement();
root.Name = htmlLines[1].Split(' ')[0];/*.Substring(0, htmlLines[1].IndexOf(' '))*/

ParseHtml(root, htmlLines.Skip(2).ToList());

Console.WriteLine("HTML Tree:");
PrintHtmlTree(root, "");

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

        var child = new HtmlElement { Name = tagName, Parent = currentParent };

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
        currentParent.Children.Add(child);

        if (!HtmlHelper.Instance.TagsNoClosing.Contains(tagName) && !line.EndsWith("/"))
            currentParent = child;
    }
    return rootElement;
}

static void PrintHtmlTree(HtmlElement element, string indentation)
{
    Console.WriteLine($"{indentation}{element.Name} (ID: {element.Id})");
    foreach (var child in element.Children)
        PrintHtmlTree(child, indentation + "  ");
}
