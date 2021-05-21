using static System.Console;
using System.Collections.Generic;
using System.Linq;

// here we'll use the same example of the open closed principle
// but implementing the composite pattern specification
// the specification classes are also changed to generic following the
// exercise here, also the & operator is added and for that
// some interfaces are changed for abstract classes
// the advantage is that before we could only compare by pairs
// now we can use the and operator with n parameters
namespace Composite.CompositeSpecification
{
 public enum Color
 {
  Red, Green, Blue
 }

 public enum Size
 {
  Small, Medium, Large
 }

 public class Product
 {
  public string Name;
  public Color Color;
  public Size Size;
  public Product(string name, Color color, Size size)
  {
   Name = name;
   Color = color;
   Size = size;
  }
 }

 //Specification enterprise pattern
 public abstract class ISpecification<T>
 {
  public abstract bool IsSatisfied(T t);

  public static ISpecification<T> operator &(ISpecification<T> first, ISpecification<T> second)
  {
   return new AndSpecification<T>(first, second);
  }
 }

 public class ColorSpecification : ISpecification<Product>
 {
  private Color color;
  public ColorSpecification(Color color)
  {
   this.color = color;
  }
  public override bool IsSatisfied(Product product)
  {
   return this.color == product.Color;
  }
 }

 public class SizeSpecification : ISpecification<Product>
 {
  private Size Size;
  public SizeSpecification(Size size)
  {
   this.Size = size;
  }
  public override bool IsSatisfied(Product product)
  {
   return this.Size == product.Size;
  }
 }

 // this is the new base class we use to apply the composite pattern
 public abstract class CompositeSpecification<T> : ISpecification<T>
 {
  protected readonly ISpecification<T>[] items;
  public CompositeSpecification(params ISpecification<T>[] items)
  {
   this.items = items;
  }
 }

 //Combinator
 public class AndSpecification<T> : CompositeSpecification<T>
 {
  public AndSpecification(params ISpecification<T>[] items) : base(items)
  {
  }

  public override bool IsSatisfied(T t)
  {
   return items.All(i => i.IsSatisfied(t));
  }
 }
 //now we only have to add new specification classes 
 //even if we have to combine criteria

 public interface IFilter<T>
 {
  IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
 }

 public class BetterFilter : IFilter<Product>
 {
  public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
  {
   foreach (var i in items)
    if (spec.IsSatisfied(i))
     yield return i;
  }
 }
 public class Main
 {
  public static void Run()
  {
   var apple = new Product("Apple", Color.Green, Size.Small);
   var tree = new Product("Tree", Color.Green, Size.Large);
   var house = new Product("House", Color.Blue, Size.Large);
   Product[] products = { apple, tree, house };

   var bf = new BetterFilter();
   WriteLine("Green Products (new): ");
   foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
    WriteLine($" - {p.Name} is green");
   WriteLine("Large Products (new): ");
   foreach (var p in bf.Filter(products, new SizeSpecification(Size.Large)))
    WriteLine($" - {p.Name} is large");

   WriteLine("Blue Large Products (new): ");
   foreach (var p in bf.Filter(products, new AndSpecification<Product>(new ISpecification<Product>[] { new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large) })))
    WriteLine($" - {p.Name} is blue and large");

   // using the & operator
   WriteLine("Green Small Products (new): ");
   foreach (var p in bf.Filter(products, (new ColorSpecification(Color.Green) & new SizeSpecification(Size.Small))))
   {
    WriteLine($" - {p.Name} is green and small");
   }
  }
 }
}