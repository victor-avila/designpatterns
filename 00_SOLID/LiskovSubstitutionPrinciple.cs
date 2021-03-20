using System;

namespace DesignPatterns
{
 public class Rectangle
 {
  public int Width { get; set; }
  public int Height { get; set; }
  public Rectangle() { }
  public Rectangle(int width, int height)
  {
   Width = width;
   Height = height;
  }
  public override string ToString()
  {
   return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
  }
 }
 public class Square : Rectangle
 {
  //new modifier hides method from base class
  public new int Width
  {
   set { base.Width = base.Height = value; }
  }
  public new int Height
  {
   set { base.Width = base.Height = value; }
  }
 }

 public class LiskovRectangle
 {
  public virtual int Width { get; set; }
  public virtual int Height { get; set; }
  public LiskovRectangle() { }
  public override string ToString()
  {
   return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
  }
 }
 public class LisvokSquare : LiskovRectangle
 {
  public override int Width
  {
   set { base.Width = base.Height = value; }
  }
  public override int Height
  {
   set { base.Width = base.Height = value; }
  }
 }
 public class LiskovSubstitutionPrinciple
 {
  public static int Area(Rectangle r) => r.Width * r.Height;
  public static int AreaLiskov(LiskovRectangle r) => r.Width * r.Height;
  public static void Run()
  {
   //be able to substitute a base type by a subtype

   //Ability to replace any instance of a parent
   //class with an instance of one of its child
   //classes without negative side effects

   //if you say that a parent type can do something on a certain way
   //all subtypes need to be able to do that with the same expectations

   Rectangle rc = new Rectangle(2, 3);
   Console.WriteLine($"{rc} has area {Area(rc)}");
   //Width: 2, Height: 3 has area 6, all good

   Square sq = new Square();
   sq.Width = 4;
   Console.WriteLine($"{sq} has area {Area(sq)}");
   //Width: 4, Height: 4 has area 16, all good

   Rectangle sq2 = new Square();
   sq2.Width = 4;
   //here the square losses its functionality
   //as the height is not set along the width
   Console.WriteLine($"{sq2} has area {Area(sq2)}");
   //Width: 4, Height: 0 has area 0, wrong 

   LiskovRectangle sq3 = new LisvokSquare();
   sq3.Width = 4;
   //here the square losses its functionality
   //as the height is not set along the width
   Console.WriteLine($"{sq3} has area {AreaLiskov(sq3)}");
   //Width: 4, Height: 4 has area 16

  }
 }
}