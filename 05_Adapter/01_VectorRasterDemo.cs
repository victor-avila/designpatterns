using static System.Console;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;

// the adapter pattern is all about conforming an interface that
// you are given to the interface that you actually have

// on this example we'll work with a simplified drawing app that only knows 
// how to draw pixels (points)



namespace Adapter.VectorRasterDemo
{
 public class Main
 {
  // here we receive a list of vector objects which we should
  // draw using our point printer (DrawPoint)
  private static readonly List<VectorObject> vectorObjects = new List<VectorObject> {
   new VectorRectangle(1, 1, 10, 10),
   new VectorRectangle(3, 3, 6, 6),
  };
  // this is the only functionality we have in our app, which
  // draw a defined point, for the example we are not considering
  // its coordinates, just showing it on console to signal that
  // a new point was drawn
  public static void DrawPoint(Point p)
  {
   Write(".");
  }
  public static void Run()
  {
   Draw();
   //this method is called twice to show one side effect of the adapter and
   //it's that it's invoked everytime it's used even if some conversions
   //are already made and it's unnecessary to process them again and
   //also generates a lot of temporary information
   Draw();
  }
  public static void Draw()
  {
   // so here we receive vector objects, but this module is oriented
   // towards the raster mindset (dot matrix or bitmap)
   foreach (var vo in vectorObjects)
   {
    foreach (var line in vo)
    {
     // here we use the adapter to convert each line to the collection
     // of points and call the Point printer
     var adapter = new LineToPointerAdapter(line);
     adapter.ToList().ForEach(DrawPoint);
    }
   }
  }
 }

 // to solve this problem we create an adapter to convert a LIne
 // to a set of points as all vector objects are made of lines
 public class LineToPointerAdapter : Collection<Point>
 {
  // here we also will keep the count of the number of invocations
  // of the line to point converion
  private static int count;
  public LineToPointerAdapter(Line line)
  {
   WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}]");
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
     Add(new Point(left, y));
    }
   }
   else if (dy == 0)
   {
    for (int x = left; x <= right; ++x)
    {
     Add(new Point(x, top));
    }
   }
  }
 }

 // lets suppose on another part of the application there is a vector
 // drawing functionality so we have all the pieces necessary to work like that

 // here we define shapes as a collections of Lines which are defined
 // by its start and end point
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
 }

 // this is a common class that uses our section of the app and
 // the vector oriented one
 public class Point
 {
  public int X, Y;
  public Point(int x, int y)
  {
   X = x;
   Y = y;
  }
 }
}
