using System;
using static System.Console;
namespace Factory.ProblemWithConstructors
{
 public enum CoordinateSystem
 {
  Cartesian,
  Polar
 }
 public class Point
 {
  private double x, y;
  //public Point(double x, double y)
  // I cannot define polar coordinates like this because
  // this signature is already used above
  //public Point(double rho, double theta)

  //one option is to send a parameter specyfing the coordinate system
  //here we also change the parameter names to make them neutral
  //we also have to document the method so that the user can understand
  //the special case of the polar system and can understand what does mean
  //a and b in every case, all this is very inconvenient
  //also with a normal constructor I'm forced to use the same
  //method name as the class so I can't specify for example a constructor called
  //InitializeAsCartesian and InitializeAsPolar
  //<summary>
  //Initializes a point from EITHER cartesian or polar
  //</summary>
  //<param name="a">x if cartesian, rho if polar</param>
  //<param name="b">y if cartesian, theta if polar</param>
  //<param name="system">coordinate system</param>
  public Point(double a, double b, CoordinateSystem system = CoordinateSystem.Cartesian)
  {
   switch (system)
   {
    case CoordinateSystem.Cartesian:
     this.x = a;
     this.y = b;
     break;
    case CoordinateSystem.Polar:
     this.x = a * Math.Cos(b);
     this.y = b * Math.Sin(b);
     break;
   }
  }

 }

 public class Main
 {
  public static void Run()
  {
   WriteLine("ProblemWithConstructors");
  }
 }
}