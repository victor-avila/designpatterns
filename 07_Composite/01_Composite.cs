using System;
using static System.Console;
using System.Collections.Generic;
using System.Text;

// emulate drawing applications, defining shapes and grouping them
// usually those applications work like that allowing to move
// or alter groups of shapes together
namespace Composite.Composite
{
 // this root class acts as a group for any kind of member that it's part of that group
 public class GraphicObject
 {
  // equals sets the default value
  public virtual string Name { get; set; } = "Group";
  // the color will be configured from the client on the intialization of the
  // children classes
  public string Color;
  // the private member and .Value on the property is to allow lazy initialization where 
  // we will instatiate the list of objects only if we need to have children
  private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
  // 
  public List<GraphicObject> Children => children.Value;
  // this recursive method will load the builder with the children's information
  // and a kind of indentation (*) to display the level of the descendant
  private void Print(StringBuilder sb, int depth)
  {
   sb.Append(new String('*', depth))
    .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color} ")
    .AppendLine(Name);
   foreach (var child in Children)
   {
    child.Print(sb, depth + 1);
   }
  }
  public override string ToString()
  {
   var sb = new StringBuilder();
   Print(sb, 0);
   return sb.ToString();
  }
 }
 public class Circle : GraphicObject
 {
  public override string Name => "Circle";
 }

 public class Square : GraphicObject
 {
  public override string Name => "Square";

 }

 public class Main
 {
  public static void Run()
  {
   var drawing = new GraphicObject { Name = "My Drawing" };
   drawing.Children.Add(new Square { Color = "Red" });
   drawing.Children.Add(new Circle { Color = "Yellow" });
   // here we see that the group is just another GraphicObject
   // containing other childern that can be shapes or other groups
   // notice that shapes also can act as groups having children
   // but we can define a group from the root class that's a graphic 
   // object but without shape
   var group = new GraphicObject();
   group.Children.Add(new Circle { Color = "Blue" });
   group.Children.Add(new Square { Color = "Blue" });
   drawing.Children.Add(group);
   // this calls the overwritten ToString() from the base class
   // which calls the recursive Print method
   WriteLine(drawing);
  }
 }
}