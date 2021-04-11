using static System.Console;
using Autofac;
// simply allows you to connect different abstraction together
namespace Bridge.Bridge
{
 public interface IRenderer
 {
  void RenderCircle(float radius);
 }
 public class VectorRenderer : IRenderer
 {
  public void RenderCircle(float radius)
  {
   WriteLine($"Drawing a circle of radius {radius}");
  }
 }
 // Raster means by pixels rendering
 public class RasterRenderer : IRenderer
 {
  public void RenderCircle(float radius)
  {
   WriteLine($"Drawing pixels for circle with radius {radius}");
  }
 }
 // here is where the bridging happens
 // instead of specifying that the shape is able to draw itself as either
 // raster form or some other form, you don't put this limitation in place
 // you don't let shape decide the different ways in it can be drawn
 // all you do is build a bridge between the shape and whoever is drawing it which
 // is an IRenderer on this case

 // the bridge pattern is a way to connecting a part of a system, on this case
 // we have circle as a domain object, and you are connecting it to the different
 // implementations of the rendering mechanism and doing it non intrusively
 // instead of giving the circle methods for drawing in raster and in vector form
 // you are providing it with the IRenderer making the bridge between the domain
 // object and the way this object should be processed
 public abstract class Shape
 {
  protected IRenderer renderer;
  protected Shape(IRenderer renderer)
  {
   this.renderer = renderer;
  }
  public abstract void Draw();
  public abstract void Resize(float factor);
 }
 public class Circle : Shape
 {
  private float radius;
  public Circle(IRenderer renderer, float radius) : base(renderer)
  {
   this.radius = radius;
  }
  public override void Draw()
  {
   // here is where we use the bridge
   renderer.RenderCircle(radius);
  }
  public override void Resize(float factor)
  {
   radius *= factor;
  }
 }
 public class Main
 {
  public static void Run()
  {
   // this way we implement it without dependency injection
   // //IRenderer renderer = new RasterRenderer();
   // var renderer = new VectorRenderer();
   // var circle = new Circle(renderer, 5);
   // circle.Draw();
   // circle.Resize(2);
   // circle.Draw();

   // this way we implement it within an dependency injection container with autofac
   var cb = new ContainerBuilder();
   cb.RegisterType<VectorRenderer>().As<IRenderer>().SingleInstance();
   // the circle is tricky because it takes an argument where we actually
   // resolve the circle from the container
   // we want to provide the argument later on, not at the point where
   // you configure the container but when you already made the container
   // and you are resolving components from it
   // here positional means that we are providing an argument by its position, 0 on this case
   cb.Register((c, p) => new Circle(c.Resolve<IRenderer>(), p.Positional<float>(0)));
   using (var c = cb.Build())
   {
    // this is the way to provide the defined positional arguments, the first
    // argument on the constructor is the position and the second the object, on
    // this case its type have to be explicit, for that reason we have to 
    // be precise with the float argument value and type
    var circle = c.Resolve<Circle>(new PositionalParameter(0, 5.0f));
    circle.Draw();
    circle.Resize(2.0f);
    circle.Draw();
   }

  }
 }
}