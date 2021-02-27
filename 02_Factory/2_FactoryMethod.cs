using System;
using static System.Console;
namespace Factory.FactoryMethod
{
 //one option to solve the problem with the cartesian and polar points
 //is to for example use inheritance inheriting PolarPoint from CartesianPoint
 //but the easier way is to use a factory
 //factory method is the simpler pattern

 //the main advantages are two
 //overload with the same set of argument types, but with
 //different descriptive names so that the api tells you the arguments you are providing
 //and the names of the factory methods are also unique
 //so that gives a hint to what kind of point the user is creating on this example
 //so in general this pattern is an api improvement
 public class Point
 {
  double x, y;
  //unlike a constructor that is forced to have the same name as the class
  //the factory method can have any name and parameter names it wants
  public static Point NewCartesianPoint(double x, double y)
  {
   return new Point(x, y);
  }

  public static Point NewPolarPoint(double rho, double theta)
  {
   return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
  }

  //now the constructor is private as it will not be invoked directly
  private Point(double x, double y)
  {
   this.x = x;
   this.y = y;
  }
  public override string ToString()
  {
   return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
  }
 }

 public class Main
 {
  public static void Run()
  {
   var point = Point.NewPolarPoint(1.0, Math.PI / 2);
   WriteLine(point);
  }
 }
}