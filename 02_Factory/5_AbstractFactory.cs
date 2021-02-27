using System;
using System.Collections.Generic;
using static System.Console;
//the difference is that you don't return concrete objects but
//abstract classes or interfaces
//on literature more people talk about the use case creating families
//of objects but that's not necessarily the case
//on the next example we'll create two separate objects, each with
//its own factory and the idea can be expanded to families if required
namespace Factory.AbstractFactory
{
 public interface IHotDrink
 {
  void Consume();
 }
 internal class Tea : IHotDrink
 {
  public void Consume()
  {
   WriteLine("This tea is nice but I'd prefer it with milk.");
  }
 }
 internal class Coffee : IHotDrink
 {
  public void Consume()
  {
   WriteLine("This coffee is sensational!");
  }
 }
 public interface IHotDrinkFactory
 {
  IHotDrink Prepare(int amount);
 }
 //here we implement the final factories that will
 //also return an interface
 //the factories are internal, are not given to anyone
 //made only to return the needed instance
 internal class TeaFactory : IHotDrinkFactory
 {
  public IHotDrink Prepare(int amount)
  {
   WriteLine($"Put in a tea bag, boil water, pur {amount} ml, add lemon, enjoy!");
   return new Tea();
  }
 }
 internal class CoffeeFactory : IHotDrinkFactory
 {
  public IHotDrink Prepare(int amount)
  {
   WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
   return new Coffee();
  }
 }
 public class HotDrinkMachine
 {
  public enum AvailableDrink
  {
   Coffee, Tea
  }
  //here we se the advantage of making the factory abstract, because we now can gather them all and use
  //them dynamically as needed, here we bind them to an enumerator and with some assembly analysis
  //we can find the respective factories by name and load them in the list to use them later
  private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();
  public HotDrinkMachine()
  {
   foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
   {
    //here Factory.AbstractFactory is the namespace where the factories are located
    var factory = (IHotDrinkFactory)Activator.CreateInstance(Type.GetType("Factory.AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory"));
    factories.Add(drink, factory);
   }
  }
  public IHotDrink MakeDrink(AvailableDrink drink, int amount)
  {
   return factories[drink].Prepare(amount);
  }
 }

 public class Main
 {
  public static void Run()
  {
   var machine = new HotDrinkMachine();
   var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea, 100);
   drink.Consume();
  }
 }
}