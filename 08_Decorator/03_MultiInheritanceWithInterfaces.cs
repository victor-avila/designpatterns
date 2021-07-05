using static System.Console;
namespace Decorator.MultiInheritanceWithInterfaces
{
 // this approach will try to emulate multiple inheritance as it is 
 // not directly support by C#, nor Java
 public class Main
 {
  // we'll create a Bird which can Fly, then a Lizard that can Crawl
  // and then a Dragon that can do both
  public interface IBird
  {
   void Fly();
   // only the Fly method will give us enough for a simple
   // implementation but we are adding the Weight property to
   // show the scenario where there are collisions
   int Weight { get; set; }
  }
  public class Bird : IBird
  {
   public int Weight { get; set; }
   public void Fly()
   {
    WriteLine($"Soaring in the sky with weight {Weight}");
   }
  }
  public interface ILizard
  {
   int Weight { get; set; }
   void Crawl();
  }
  public class Lizard
  {
   public int Weight { get; set; }
   public void Crawl()
   {
    WriteLine($"Crawling in the dirt with weight {Weight}");
   }
  }
  // here if we just inherited from Bird and Lizard we'll have
  // to duplicate the code for Fly and Crawl, so to avoid that
  // we use decorator to be able to forward the call to the methods
  // from each corresponding class
  public class Dragon : IBird, ILizard
  {
   // here we implement the decorator pattern creating an instance
   // of each of the classes we'll be delegating to
   // we are also initializing in place but for demo purposes, usually
   // you would do this on the constructor so that we implement
   // a dependency injection framework later 
   private Bird bird = new Bird();
   private Lizard lizard = new Lizard();
   private int weight;
   // with Resharper we use Delegate Members to generate the follwing
   // methods automatically
   public void Crawl()
   {
    lizard.Crawl();
   }
   public void Fly()
   {
    bird.Fly();
   }
   // to address the collision we are creating a custom
   // Weight property that will handle both delegated classes
   // this whole strategy is very inconvenient to work with as
   // can be seen by this situation that's pretty common with multiple
   // inheritance
   public int Weight
   {
    get { return weight; }
    set
    {
     weight = value;
     bird.Weight = value;
     lizard.Weight = value;
    }
   }
  }

  public static void Run()
  {
   var d = new Dragon();
   d.Weight = 123;
   d.Fly();
   d.Crawl();
  }
 }
}