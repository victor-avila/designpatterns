using System;
using System.Text;
using System.Collections.Generic;
using static System.Console;
namespace Builder.Builder
{
 public class Builder
 {
  public class HtmlElement
  {
   public string Name, Text;
   public List<HtmlElement> Elements = new List<HtmlElement>();
   private const int indentSize = 2;
   public HtmlElement() { }
   public HtmlElement(string name, string text)
   {
    Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
    Text = text ?? throw new ArgumentNullException(paramName: nameof(text));
   }
   private string ToStringImpl(int indent)
   {
    var sb = new StringBuilder();
    var i = new string(new string(' ', indentSize * indent));
    sb.AppendLine($"{i}<{Name}>");
    if (!string.IsNullOrWhiteSpace(Text))
    {
     sb.Append(new string(' ', indentSize * (indent + 1)));
     sb.AppendLine(Text);
    }
    foreach (var e in Elements)
    {
     sb.Append(e.ToStringImpl(indent + 1));
    }
    sb.AppendLine($"{i}</{Name}>");
    return sb.ToString();
   }
   public override string ToString()
   {
    return ToStringImpl(0);
   }
  }
  public class HtmlBuilder
  {
   private readonly string rootName;
   HtmlElement root = new HtmlElement();
   public HtmlBuilder(string rootName)
   {
    this.rootName = rootName;
    root.Name = rootName;
   }
   public void AddChild(string childName, string childText)
   {
    var e = new HtmlElement(childName, childText);
    root.Elements.Add(e);
   }
   public override string ToString()
   {
    return root.ToString();
   }
   // this is optional to reuse the same builder
   // to construct more other objects
   public void Clear()
   {
    root = new HtmlElement { Name = rootName };
   }
  }
  public static void Run()
  {
   var sb = new StringBuilder();
   var words = new[] { "hello", "world" };
   sb.Append("<ul>");
   foreach (var word in words)
   {
    sb.AppendFormat("<li>{0}</li>", word);
   }
   sb.Append("</ul>");
   WriteLine(sb);

   var builder = new HtmlBuilder("ul");
   builder.AddChild("li", "hello");
   builder.AddChild("li", "world");
   WriteLine(builder.ToString());
  }
 }
}