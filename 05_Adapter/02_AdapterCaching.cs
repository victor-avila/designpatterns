using static System.Console;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Collections;

namespace Adapter.AdapterCaching
{
 // one side effect of the adapter pattern is that you generate
 // a lot of temporary information
 // caching is simply to preserve the information that you stored
 // for future use
 public class Main
 {
  private static readonly List<VectorObject> vectorObjects = new List<VectorObject> {
   new VectorRectangle(1, 1, 10, 10),
   new VectorRectangle(3, 3, 6, 6),
  };
  public static void DrawPoint(Point p)
  {
   Write(".");
  }
  public static void Run()
  {
   Draw();
   Draw();
  }
  public static void Draw()
  {
   foreach (var vo in vectorObjects)
   {
    foreach (var line in vo)
    {
     var adapter = new LineToPointerAdapter(line);
     adapter.ToList().ForEach(DrawPoint);
    }
   }
  }
 }
 // here we changed the inheritance from Collection<Point>
 // to IEnumerable<Point> having to implement GetEnumerator()
 public class LineToPointerAdapter : IEnumerable<Point>
 {
  private static int count;

  // here we defined the cache with a unique identifier that's a 
  // calculated hashcode for the line, and each one have 
  // the list of points that compose it
  static Dictionary<int, List<Point>> cache = new Dictionary<int, List<Point>>();

  public LineToPointerAdapter(Line line)
  {
   // here we just avoid recalculating if this particular line
   // is already on the cache  
   var hash = line.GetHashCode();
   if (cache.ContainsKey(hash)) return;
   // without caching we'll be calling 16 times this line
   // 8 by each call to Draw()
   WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}]");
   // here the difference is that before we were adding the points
   // to ourselves (this class instance), instead we'll be adding
   // them to the list of points that we will store on the cache
   var points = new List<Point>();
   int left = Math.Min(line.Start.X, line.End.X);
   int right = Math.Max(line.Start.X, line.End.X);
   int top = Math.Min(line.Start.Y, line.End.Y);
   int bottom = Math.Max(line.Start.X, line.End.Y);
   int dx = right - left;
   int dy = line.End.Y - line.Start.Y;
   if (dx == 0)
   {
    for (int y = top; y <= bottom; ++y)
    {
     points.Add(new Point(left, y));
    }
   }
   else if (dy == 0)
   {
    for (int x = left; x <= right; ++x)
    {
     points.Add(new Point(x, top));
    }
   }
   // and here we add the entry for this line
   // to our cache
   cache.Add(hash, points);
  }
  // here we just implement the enumerator returning
  // the cache values
  public IEnumerator<Point> GetEnumerator()
  {
   return cache.Values.SelectMany(x => x).GetEnumerator();
  }
  IEnumerator IEnumerable.GetEnumerator()
  {
   return GetEnumerator();
  }
 }
 public class VectorRectangle : VectorObject
 {
  public VectorRectangle(int x, int y, int width, int height)
  {
   Add(new Line(new Point(x, y), new Point(x + width, y)));
   Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
   Add(new Line(new Point(x, y), new Point(x, y + height)));
   Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
  }
 }
 public class VectorObject : Collection<Line>
 {
 }
 public class Line
 {
  public Point Start, End;
  public Line(Point start, Point end)
  {
   Start = start;
   End = end;
  }
  // the same as the point, we get the equality functionality
  // with resharper
  protected bool Equals(Line other)
  {
   return Equals(Start, other.Start) && Equals(End, other.End);
  }
  public override bool Equals(object obj)
  {
   if (ReferenceEquals(null, obj)) return false;
   if (ReferenceEquals(this, obj)) return true;
   if (obj.GetType() != this.GetType()) return false;
   return Equals((Line)obj);
  }
  public override int GetHashCode()
  {
   unchecked
   {
    return ((Start != null ? Start.GetHashCode() : 0) * 397) ^ (End != null ? End.GetHashCode() : 0);
   }
  }
 }
 public class Point
 {
  public int X, Y;
  public Point(int x, int y)
  {
   X = x;
   Y = y;
  }
  // all the code bellow is obtained using the resharper generate equality
  // members functionality, on this exercise we are interested only
  // in the GetHashCode method to generate a unique identifier
  protected bool Equals(Point other)
  {
   return X == other.X && Y == other.Y;
  }
  public override bool Equals(object obj)
  {
   if (ReferenceEquals(null, obj)) return false;
   if (ReferenceEquals(this, obj)) return true;
   if (obj.GetType() != this.GetType()) return false;
   return Equals((Point)obj);
  }
  public override int GetHashCode()
  {
   unchecked
   {
    return (X * 397) ^ Y;
   }
  }
 }
}