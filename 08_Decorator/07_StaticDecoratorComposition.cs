using static System.Console;
// all this is just a demonstration trying to implement something
// like this on C# using abstract classes instead of interfaces
// var circle = new TransparentShape<ColoredShape<Circle>>(0.4f);
// but as can be seen it's not that useful as we can't pass the
// parameters to the constructors for ColoredShape or Circle
// even so it can be done on C++
namespace Decorator.StaticDecoratorComposition
{
 public abstract class Shape
 {
  public abstract string AsString();
 }
 public class Circle : Shape
 {
  private float radius;
  // even if we add this properties, we are not able to use them
  // as we are not using inheritance, so this will not work:
  // var circle = new TransparentShape<ColoredShape<Circle>>(0.4f);
  // circle.Radius = 5.0f;
  public float Radius
  {
   get => radius;
   set => radius = value;
  }
  public Circle() : this(0.0f) { }
  public Circle(float radius)
  {
   this.radius = radius;
  }
  public void Resize(float factor)
  {
   radius *= factor;
  }
  public override string AsString() => $"A circle with radius {radius}";
 }

 public class Square : Shape
 {
  public float side;
  // we have to add this default constructors on all classes as
  // on c# we cannot setup constructor forwarding, you cannot proxy 
  // for example a constructor parameter from ColorShape into Square 
  // for the following example:
  // var redSquare = new ColoredShape<Square>("red");
  // as can be seen we can only pass the parameter for the ColoredShape constructor
  // but not for the Square, then we need a default constructor on Square
  public Square() : this(0.0f) { }
  public Square(float side)
  {
   this.side = side;
  }
  public override string AsString() => $"A square with side {side}";
 }

 public class ColoredShape : Shape
 {
  private Shape shape;
  private string color;
  public ColoredShape(Shape shape, string color)
  {
   this.shape = shape;
   this.color = color;
  }
  public override string AsString() => $"{shape.AsString()} has the color {color}";
 }

 public class TransparentShape : Shape
 {
  private Shape shape;
  private float transparency;
  public TransparentShape(Shape shape, float transparency)
  {
   this.shape = shape;
   this.transparency = transparency;
  }
  public override string AsString() => $"{shape.AsString()} has {transparency * 100.0}% transparency";
 }

 // this is not supported on C# but it is on C++
 // (being able to inherit from a template argument on in this case a generic parameter)
 // it's called Curiously Recurrent Template Pattern (CRTP)
 //public class ColoredShape<T> : T

 // new() means that it have a default constructor
 public class ColoredShape<T> : Shape where T : Shape, new()
 {
  private string color;
  // with this instance we are using aggregation  to emulate the CRTP
  T shape = new T();

  public ColoredShape() : this("black") { }

  public ColoredShape(string color)
  {
   this.color = color;
  }

  public override string AsString() => $"{shape.AsString()} has the color {color}";
 }

 public class TransparentShape<T> : Shape where T : Shape, new()
 {
  public float transparency;
  T shape = new T();

  public TransparentShape() : this(0.0f) { }

  public TransparentShape(float transparency)
  {
   this.transparency = transparency;
  }

  public override string AsString() => $"{shape.AsString()} has {transparency * 100.0f}% transparency";
 }

 public class Main
 {
  public static void Run()
  {
   //TransparentShape<ColoredShape<Square>> shape();
   var redSquare = new ColoredShape<Square>("red");
   WriteLine(redSquare.AsString());
   var circle = new TransparentShape<ColoredShape<Circle>>(0.4f);
   // the following doesn't work
   // circle.Radius = 5.0f;
   WriteLine(circle.AsString());


  }
 }
}