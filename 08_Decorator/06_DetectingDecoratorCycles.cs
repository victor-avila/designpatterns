using static System.Console;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

// here we want to prevent the user from applying the same decorator
// twice for example with the color red and blue at the same time
// as this decorator is dynamic we have to check at runtime
// it is a little overengineered but it's useful to se how to handle
// those complex situations using policies making use of the strategy pattern
namespace Decorator.DetectingDecoratorCycles
{
 public abstract class Shape
 {
  public virtual string AsString() => string.Empty;
 }

 public sealed class Circle : Shape
 {
  private float radius;

  public Circle() : this(0)
  {
  }

  public Circle(float radius)
  {
   this.radius = radius;
  }

  public void Resize(float factor)
  {
   radius *= factor;
  }

  public override string AsString() => $"A circle of radius {radius}";
 }

 public sealed class Square : Shape
 {
  private readonly float side;

  public Square() : this(0)
  {
  }

  public Square(float side)
  {
   this.side = side;
  }

  public override string AsString() => $"A square with side {side}";
 }

 public abstract class ShapeDecoratorCyclePolicy
 {
  // this method is to validate on the constructor
  // the list contains the chain of decorator types already added
  public abstract bool TypeAdditionAllowed(Type type, IList<Type> allTypes);
  // this one is to check on the moment where the decorator is applied
  // in case we don't want to prevent it but to swallow it silently or some
  // other strategy
  public abstract bool ApplicationAllowed(Type type, IList<Type> allTypes);
 }

 // with this one we will throw an exception in case the user is
 // trying to add a decorator already added
 public class ThrowOnCyclePolicy : ShapeDecoratorCyclePolicy
 {
  // this method is there only to be reused by the other two
  // as will behave the same
  private bool handler(Type type, IList<Type> allTypes)
  {
   if (allTypes.Contains(type))
    throw new InvalidOperationException(
      $"Cycle detected! Type is already a {type.FullName}!");
   return true;
  }

  public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
  {
   return handler(type, allTypes);
  }

  public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
  {
   return handler(type, allTypes);
  }
 }

 // this policy allows the repeated decorator but ignores its
 // application and leaves the previous one
 public class AbsorbCyclePolicy : ShapeDecoratorCyclePolicy
 {
  public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
  {
   return true;
  }

  public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
  {
   return !allTypes.Contains(type);
  }
 }

 // this policy just allows everything
 public class CyclesAllowedPolicy : ShapeDecoratorCyclePolicy
 {
  public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
  {
   return true;
  }

  public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
  {
   return true;
  }
 }

 // the following abstract clases is a recurrent strategy on .net
 // when creating libraries, we create its generic and nongeneric form:
 // Foo, Foo<T> : Foo
 // this is because we can't run the following code
 // if (x is Foo<> f) where the generic type is open, it has to be specified

 public abstract class ShapeDecorator : Shape
 {
  // here we have the list of types applied
  protected internal readonly List<Type> types = new();
  // here we have internal object to implement the decorator pattern
  protected internal Shape shape;

  // non-generic version
  public ShapeDecorator(Shape shape)
  {
   // by default in the non-gen version it will add the decorator
   // it doesn't matter if it's already there as we have not
   // specified any policy
   this.shape = shape;
   if (shape is ShapeDecorator sd)
    types.AddRange(sd.types);
  }
 }

 // generic version
 public abstract class ShapeDecorator<TSelf, TCyclePolicy> : ShapeDecorator
   where TCyclePolicy : ShapeDecoratorCyclePolicy, new()
 {
  protected readonly TCyclePolicy policy = new();

  public ShapeDecorator(Shape shape) : base(shape)
  {
   // here we are validating against the policy definition
   if (policy.TypeAdditionAllowed(typeof(TSelf), types))
    types.Add(typeof(TSelf));
  }
 }

 // this optional base class will set a policy globally by 
 // default if implemented on the decorator
 public class ShapeDecoratorWithPolicy<T>
   : ShapeDecorator<T, ThrowOnCyclePolicy>
 {
  public ShapeDecoratorWithPolicy(Shape shape) : base(shape)
  {
  }
 }

 // dynamic
 public class ColoredShape
   : ShapeDecorator<ColoredShape, AbsorbCyclePolicy>
 // this can be used instead of the previous line to set a policy globally,
 // on this case the one implemented on the base class ShapeDecoratorWithPolicy
 // which is ThrowOnCyclePolicy
 // : ShapeDecoratorWithPolicy<ColoredShape> 

 {

  private readonly string color;

  public ColoredShape(Shape shape, string color) : base(shape)
  {
   this.color = color;
  }

  public override string AsString()
  {
   var sb = new StringBuilder($"{shape.AsString()}");

   // here we validate if the decorator should be effectively applied
   // types[0] -> current
   // types[1...] -> what it wraps
   if (policy.ApplicationAllowed(types[0], types.Skip(1).ToList()))
    sb.Append($" has the color {color}");

   return sb.ToString();
  }
 }

 public class Main
 {
  public static void Run()
  {
   var circle = new Circle(2);
   // we want to protect against the user setting the same decorator twice
   var colored1 = new ColoredShape(circle, "red");
   var colored2 = new ColoredShape(colored1, "blue");

   WriteLine(circle.AsString());
   WriteLine(colored1.AsString());
   WriteLine(colored2.AsString());
  }
 }

}