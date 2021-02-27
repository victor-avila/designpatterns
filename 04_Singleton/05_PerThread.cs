using static System.Console;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Singleton.PerThread
{
 //before we used Lazy<T> which gave us thread safety during initialization, apart
 //from the lazy loading
 //but sometimes you would need a singleton per thread instead of per app domain
 //on .net we have [ThreadStatic] as a global thing, but with 
 //ThreadLocal<> it's per thread, and its implementation is similar to Lazy
 //it's up to the developer to find scenarios where this is useful as the usual
 //is to require thread safety with a global singleton instance
 //this kind of lifetime is also available when you configure an IoC (Inversion of Control) container (aka DI Container)
 //so with the dependency injection container you can setup a lifetime of Per Thread
 //here is the approach to do it manually without a container
 public sealed class PerThreadSingleton
 {
  //instead of use Lazy we'll use a ThreadLocal variable
  private static ThreadLocal<PerThreadSingleton> threadInstance = new ThreadLocal<PerThreadSingleton>(() => new PerThreadSingleton());
  public int Id;
  //private constructor as with the other singleton examples
  private PerThreadSingleton()
  {
   //this is just to be able to identify the generated Id of the thread
   //in wich the object was constructed
   Id = Thread.CurrentThread.ManagedThreadId;
  }
  //this is how we expose it
  public static PerThreadSingleton Instance => threadInstance.Value;
 }

 public class Main
 {
  public static void Run()
  {
   var t1 = Task.Factory.StartNew(() =>
   {
    WriteLine("t1: " + PerThreadSingleton.Instance.Id);
   });
   var t2 = Task.Factory.StartNew(() =>
   {
    WriteLine("t2: " + PerThreadSingleton.Instance.Id);
    //this  second call is just to verify that we don't
    //get another instance on that thread
    WriteLine("t2: " + PerThreadSingleton.Instance.Id);
   });
   Task.WaitAll(t1, t2);
   //here the order and values can be different because those are parallel threads
   //the important thing is that t2 has the same id on both cases and it's different from t1
   //t2: 4
   //t1: 5
   //t2: 4
  }
 }
}