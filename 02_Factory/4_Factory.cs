using System;
using static System.Console;

namespace Factory.Factory
{
 public class Point
 {
  double x, y;
  private Point(double x, double y)
  {
   this.x = x;
   this.y = y;
  }
  public override string ToString()
  {
   return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
  }
  //the Factory exists to separate the actual object from its construction
  //also to release the object of the burden of its construction
  //when there are many different ways to do it
  //the Factory is an internal class of the Point class so it can
  //have access to the Point constructor as it is private to avoid calling it
  //directly, so that no one can use var p = new Point();
  //this approach is used in the TPL (Task Parallel Library) from .NET
  //ex. Task.Factory.StartNew(...)
  //another option is to locate the factory outside and make the
  //Point constructor internal so that other dlls can't access it, but
  //it will be accessible for the other modules inside the namespace 
  //this patterns is not official on the gang of four, only factory method and abstract factory
  public static class Factory
  {
   public static Point NewCartesianPoint(double x, double y)
   {
    return new Point(x, y);
   }
   public static Point NewPolarPoint(double rho, double theta)
   {
    return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
   }
  }
 }

 public class Main
 {
  public static void Run()
  {
   Point p = Point.Factory.NewCartesianPoint(3, 4);
   WriteLine(p);
  }
 }
}