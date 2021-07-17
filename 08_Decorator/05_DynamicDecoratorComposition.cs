using static System.Console;

// here we will compose a decorator on a decorator on a particular class
namespace Decorator.DynamicDecoratorCompostion
{
 public interface IShape
 {
  string AsString();
 }
 public class Circle : IShape
 {
  private float radius;
  public Circle(float radius)
  {
   this.radius = radius;
  }
  public void Resize(float factor)
  {
   radius *= factor;
  }
  public string AsString() => $"A circle with radius {radius}";
 }

 public class Square : IShape
 {
  public float side;
  public Square(float side)
  {
   this.side = side;
  }
  public string AsString() => $"A square with side {side}";
 }

 // here we have the first decorator as usual, we add the new
 // property (color) and rely on the methods of the shape instance
 public class ColoredShape : IShape
 {
  private IShape shape;
  private string color;
  public ColoredShape(IShape shape, string color)
  {
   this.shape = shape;
   this.color = color;
  }
  public string AsString() => $"{shape.AsString()} has the color {color}";
 }

 public class TransparentShape : IShape
 {
  private IShape shape;
  private float transparency;
  public TransparentShape(IShape shape, float transparency)
  {
   this.shape = shape;
   this.transparency = transparency;
  }
  public string AsString() => $"{shape.AsString()} has {transparency * 100.0}% transparency";
 }

 public class Main
 {
  public static void Run()
  {
   var square = new Square(1.23f);
   WriteLine(square.AsString());

   // here we use the first decorator
   var redSquare = new ColoredShape(square, "red");
   WriteLine(redSquare.AsString());

   // here we use the second decorator on runtime,
   // the inner call to AsString will be the one from the first
   // decorator because even if we are using an IShape parameter type
   // in reality the instance is being send as a concrete ColoredShape
   // the result will be
   // A square with side 1.23 has the color red has 50% transparency
   var redHalfTransparentSquare = new TransparentShape(redSquare, 0.5f);
   WriteLine(redHalfTransparentSquare.AsString());
   // one problem with this is that you can't prevent the use of the
   // same decorator twice, for example a ColoredShape of a ColoredShape
   // although you can control it making a list with all the decorators
   // on the chain, but this simpler solution is generally good enough
  }
 }
}