using System;
using System.Collections.Generic;

namespace DesignPatterns
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

 public class ProductFilter
 {
  public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
  {
   foreach (var p in products)
    if (p.Size == size)
     yield return p;
  }
  public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
  {
   foreach (var p in products)
    if (p.Color == color)
     yield return p;
  }
  public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
  {
   foreach (var p in products)
    if (p.Size == size && p.Color == color)
     yield return p;
  }
  //we have to keep altering the original class adding filters as required
  //also all the combinations potentially multiply the required methods
 }

 //Specification enterprise pattern
 public interface ISpecification<T>
 {
  bool IsSatisfied(T t);
 }

 public class ColorSpecification : ISpecification<Product>
 {
  private Color color;
  public ColorSpecification(Color color)
  {
   this.color = color;
  }
  public bool IsSatisfied(Product product)
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
  public bool IsSatisfied(Product product)
  {
   return this.Size == product.Size;
  }
 }

 //Combinator
 public class AndSpecification : ISpecification<Product>
 {
  private ISpecification<Product> Specification1;
  private ISpecification<Product> Specification2;

  public AndSpecification(ISpecification<Product> spec1, ISpecification<Product> spec2)
  {
   this.Specification1 = spec1;
   this.Specification2 = spec2;
  }

  public bool IsSatisfied(Product product)
  {
   return this.Specification1.IsSatisfied(product) && this.Specification2.IsSatisfied(product);
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

 public class OpenClosedPrinciple
 {
  public static void Run()
  {
   var apple = new Product("Apple", Color.Green, Size.Small);
   var tree = new Product("Tree", Color.Green, Size.Large);
   var house = new Product("House", Color.Blue, Size.Large);
   Product[] products = { apple, tree, house };
   var pf = new ProductFilter();
   Console.WriteLine("Green Products (old): ");
   foreach (var p in pf.FilterByColor(products, Color.Green))
    Console.WriteLine($" - {p.Name} is green");

   var bf = new BetterFilter();
   Console.WriteLine("Green Products (new): ");
   foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
    Console.WriteLine($" - {p.Name} is green");
   Console.WriteLine("Large Products (new): ");
   foreach (var p in bf.Filter(products, new SizeSpecification(Size.Large)))
    Console.WriteLine($" - {p.Name} is large");
   Console.WriteLine("Blue Large Products (new): ");
   foreach (var p in bf.Filter(products, new AndSpecification(new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large))))
    Console.WriteLine($" - {p.Name} is blue and large");
  }
 }

}
