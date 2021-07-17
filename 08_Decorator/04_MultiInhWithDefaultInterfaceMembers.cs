using static System.Console;
// multiple inheritance got an additional twist with
// the realease of c# 8 because now with 
// default interface members we can add behaviour
// directly to the interface so that we can add many
// behaviours to a clase implementing interfaces

// extension methods achieve something similar adding
// behaviour to an existing class but without altering
// the type while with default interface members we have
// to change it to add the interfaces implementation
namespace Decorator.MultiInhWithDefaultInterfaceMembers
{
 public interface ICreature
 {
  int Age { get; set; }
 }

 public interface IBird : ICreature
 {
  // here the default interface member definition
  public void Fly()
  {
   if (Age >= 10)
    WriteLine("I am flying");
  }
 }

 public interface ILizard : ICreature
 {
  public void Crawl()
  {
   if (Age < 10)
    WriteLine("I am crawling");
  }
 }

 public class Dragon : IBird, ILizard
 {
  public int Age { get; set; }
 }

 // options to add behaviours on the side to dragon
 // inheritance if we can afford it
 // SmartDragon(Dragon) - wrapper class around dragon
 // extension method
 // C#8 default interface methods - this one

 public static class Main
 {
  public static void Run()
  {
   Dragon d = new Dragon { Age = 5 };
   // this way we cannot just call d.Crawl() as the method is not
   // part of the Dragon class, so we can cast like this:
   if (d is ILizard lizard)
    lizard.Crawl();
   d.Age = 20;
   if (d is IBird bird)
    bird.Fly();
  }
 }
}