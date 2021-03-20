using static System.Console;
using System.Collections.Generic;
using System;
// on this example there will be implemented 3 different things, in reality
// the generic value adapter is just about the silly thing we have to do
// in C# in order to propagate literal values like an int value as arguments
// of a generic type
// also there are addional topics like how to expand the hirarchy (partial specialization)
// so that you get operations on numeric types on this case
// an the last one is how to make a factory method which uses
// recursive generics to propagate type information accross the inheritance hirarchy
namespace Adapter.GenericValueAdapter
{
 // this is a variation of the adapter pattern
 // not necessary on languages like C++ but it is on C#
 // on the exercise we will implement vectors on a n-dimentional space and
 // also can have different types (int, decimal, float, or something else)
 // so at the end we want a class like Vector2f (2 dimensions and float) or Vector3i

 // this section here is really the generic value adapter
 // where we adapt a literal value (on this case an int)
 // to a type, so we can use it later on a generic definition
 // on this case setting the size of the array
 public interface IInteger
 {
  int Value { get; }
 }

 // we could create a syntetic class or a namespace to wrap
 // this implementation, but on this case for brevity we will
 // just use an static class
 public static class Dimensions
 {
  // here we only yield the actual 
  // value on the getter
  public class Two : IInteger
  {
   public int Value => 2;
  }
  public class Three : IInteger
  {
   public int Value => 3;
  }
 }

 public class Vector<TSelf, T, D>
  where D : IInteger, new()
  where TSelf : Vector<TSelf, T, D>, new()
 {
  protected T[] data;
  public Vector()
  {
   // although the video mentions that the D type carries the number of dimensions
   // this implementation really sets the size of the one-dimensional array, so I suppose
   // it's a simplification to focus on the actual pattern instead of its usability or
   // all we can hold here is the coordinates of a point on an n-dimentional space, for
   // example on 3D the size of the array will be 3 where T[0] is x, T[1] is y and T[2] is z
   data = new T[new D().Value];
  }
  // this default constructor will work but the next factory method
  // will allow to to return the class inheriting from here, the two
  // do the same at the end
  public Vector(params T[] values)
  {
   var requiredSize = new D().Value;
   data = new T[requiredSize];
   var providedSize = values.Length;
   for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
    data[i] = values[i];
  }
  public static TSelf Create(params T[] values)
  {
   var result = new TSelf();
   var requiredSize = new D().Value;
   result.data = new T[requiredSize];
   var providedSize = values.Length;
   for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
    result.data[i] = values[i];
   return result;
  }
  // here we add an indexer to address the different elements
  // so we can do something like v[0] = 12;
  public T this[int index]
  {
   get => data[index];
   set => data[index] = value;
  }
 }

 // to implement the operator plus (+) we have to do partial specialization
 // as it can't be done above directly with generics, here we have
 // access to the final type (int) and can sum its values, so this intermediate
 // class is defined only to implement the operator plus on this case
 public class VectorOfInt<D> : Vector<VectorOfInt<D>, int, D>
  where D : IInteger, new()
 {
  public VectorOfInt()
  {
  }
  public VectorOfInt(params int[] values) : base(values)
  {
  }
  // lhs and rhs stand for Left and Right Hand Side
  public static VectorOfInt<D> operator +(VectorOfInt<D> lhs, VectorOfInt<D> rhs)
  {
   var result = new VectorOfInt<D>();
   var dim = new D().Value;
   for (int i = 0; i < dim; i++)
   {
    result[i] = lhs[i] + rhs[i];
   }
   return result;
  }
 }
 public class Vector2i : VectorOfInt<Dimensions.Two>
 {
  public Vector2i()
  {
  }
  public Vector2i(params int[] values) : base(values)
  {
  }
 }

 // for VectorOfFloat and Vector3f we don't implement the constructors as we will
 // use the factory method defined on Vector
 public class VectorOfFloat<TSelf, D> : Vector<TSelf, float, D>
  where D : IInteger, new()
  where TSelf : Vector<TSelf, float, D>, new()
 {
 }
 // here we declare the inheritance sending what type will be TSelf (<Vector3f,)
 public class Vector3f : VectorOfFloat<Vector3f, Dimensions.Three>
 {
 }

 public class Main
 {
  public static void Run()
  {
   var v = new Vector2i();
   v[0] = 0;
   var vv = new Vector2i();
   // here we take advantage of the plus operator defined on VectorOfInt
   var result = v + vv;
   // al the TSelf thing is called recursive generics that is a 
   // rare approach where we propagate additional type information
   // up to the base class so that the factory method 
   // defined on Vector returns the inner most inherited class,
   // Vector3f on this case and we don't have to define 
   // the constructors for each class inheriting from there as with Vector2i,
   // the chain is Vector > VectorOfFloat > Vector3f
   Vector3f u = Vector3f.Create(3.5f, 2.2f, 1);
  }
 }
}