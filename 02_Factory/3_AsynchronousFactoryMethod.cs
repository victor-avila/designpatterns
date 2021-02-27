using System;
using System.Threading.Tasks;
using static System.Console;
namespace Factory.AsynchronousFactoryMethod
{

 //asynchronous invocation can't happen in constructors
 //without this pattern you end up with something like this
 //var foo = new Foo();
 //await foo.InitAsync();
 //there we have to remember to call the initialize method
 //using the factory method we will prohibit the use of the constructor directly
 //so that the user will be forced to use the factory method to create new instances of the class
 //and at the same time it can be asynchronous and fluent

 public class Foo
 {
  //on the constructor you can't await, so it can't have async code
  //await expression can only be used in a method or lambda mark with async modifier
  //also we make it private so that no one can construct the object directly
  private Foo()
  {
   //
  }
  //the use of generics here is to make it fluent, this is the
  //internal implementation of the constructor
  private async Task<Foo> InitAsync()
  {
   await Task.Delay(1000);
   return this;
  }
  //this is the factory method, the same, it uses generics
  //to make it fluent
  public static Task<Foo> CreateAsync()
  {
   var result = new Foo();
   return result.InitAsync();
  }

 }

 public class AsynchronousFactoryMethod
 {
  public static void Run()
  {
   //now we can create and initialize the object with only one line
   //and which is fluent and asynchronous
   Foo x = Foo.CreateAsync().Result;
   WriteLine(x);
  }
 }
}