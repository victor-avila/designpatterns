using System;
using System.Text;
using System.Collections.Generic;
using static System.Console;
namespace Builder.FluentBuilder
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
   //this is the only method that changes respecto to a normal Builder
   //we just return a reference to the same builder so that we can chain calls
   public HtmlBuilder AddChild(string childName, string childText)
   {
    var e = new HtmlElement(childName, childText);
    root.Elements.Add(e);
    return this;
   }
   public override string ToString()
   {
    return root.ToString();
   }
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
   //here we are chaining the calls to AddChild
   builder.AddChild("li", "hello").AddChild("li", "world");
   WriteLine(builder.ToString());
  }
 }
}