using static System.Console;
using System.Collections.Generic;
using System;
using System.Text;

namespace Singleton.AmbientContext
{
 public class Main
 {
  public static void Run()
  {
   // here we define a building that it's a set of walls
   // each with its position and dimentions
   // also we group them by floor with the condition
   // that all walls on the same floor should have the same
   // height
   var house = new Building();
   using (new BuildingContext(3000))
   {
    // ground floor WallHeight 3000
    house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 40000)));
    // next we define an internal context with a scope to work on the first
    // floor so that all walls will have the height defined
    // on the building context and once out of the scope
    // it should return to the previous height
    using (new BuildingContext(3500))
    {
     // first floor WallHeight 3500
     house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
     house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 40000)));
    }
    // ground floor WallHeight 3000
    house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
   }
   WriteLine(house);
  }
 }

 // by ambient it refers to values that have to be present
 // everywhere, one way for example would be to pass the height
 // as a parameter on the constructor of every wall, but as its
 // used everywhere we can centralize it on a context where we 
 // can keep certain values
 // this approach is not thread safe but it can be easily solved
 // with the [ThreadStatic] attribute or the ThreadLocal type you
 // can use for storing the context
 // the ambient context pattern is not strictly a pure singleton but
 // its a defacto singleton as its based on the same concept of having
 // a static value available at any time, just that here it's
 // layered in different contexts
 public sealed class BuildingContext : IDisposable
 {
  public int WallHeight;
  // here we need a stack to store the chain of contexts in case
  // there are used many nested scopes
  private static Stack<BuildingContext> stack = new Stack<BuildingContext>();
  // it's uncommon to have static constructors, its an action that needs
  // to be executed once only, it's called automatically before the first
  // instance is created or any static members are referenced
  static BuildingContext()
  {
   // we initialize the stack here so there is at least one
   // value in any given time
   stack.Push(new BuildingContext(0));
  }
  public BuildingContext(int wallHeight)
  {
   WallHeight = wallHeight;
   // adding yourself to the stack
   stack.Push(this);
  }
  // to get the current element we use peek to 
  // bring the element on the top of the stack
  public static BuildingContext Current => stack.Peek();
  public void Dispose()
  {
   // here we only remove the latest element
   if (stack.Count > 1)
    stack.Pop();
  }
 }

 public class Building
 {
  public List<Wall> Walls = new List<Wall>();
  public override string ToString()
  {
   var sb = new StringBuilder();
   foreach (var wall in Walls)
   {
    sb.AppendLine(wall.ToString());
   }
   return sb.ToString();
  }
 }

 public class Wall
 {
  //start and end positions of the wall plus the height
  //making it 2d
  public Point Start, End;
  public int Height;
  public Wall(Point start, Point end)
  {
   this.Start = start;
   this.End = end;
   // this way we use the top element of the stack for the height
   // which should be the current height defined for this context
   this.Height = BuildingContext.Current.WallHeight;
  }
  public override string ToString()
  {
   return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}, {nameof(Height)}: {Height}";
  }
 }
 public class Point
 {
  private int x, y;
  public Point(int x, int y)
  {
   this.x = x;
   this.y = y;
  }
  public override string ToString()
  {
   return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
  }
 }
}