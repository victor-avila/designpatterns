using System;
using System.Collections.Generic;
using static System.Console;

// the previous example violates the Open Closed Principle by defining
// the enumerator that it's inside the HotDrinkMachine, because if we want
// to aggregate other type of drink like Chocolate, then we'll have to
// modify directly the class, to address it we can use reflection

namespace Factory.AbstractFactoryOCP
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
  // public enum AvailableDrink
  // {
  //  Coffee, Tea
  // }
  // private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();
  // public HotDrinkMachine()
  // {
  //  foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
  //  {
  //   var factory = (IHotDrinkFactory)Activator.CreateInstance(Type.GetType("Factory.AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory"));
  //   factories.Add(drink, factory);
  //  }
  // }
  // public IHotDrink MakeDrink(AvailableDrink drink, int amount)
  // {
  //  return factories[drink].Prepare(amount);
  // }


  // typically you we use a dependency injection container, but for this example
  // we'll use reflection to exemplify

  private List<Tuple<string, IHotDrinkFactory>> factories = new List<Tuple<string, IHotDrinkFactory>>();

  // here we use a tuple to store the name of the class to build and its corresponding factory
  public HotDrinkMachine() {
   //this is to load all the types where the HotDrinkMachine is contained
   foreach(var t in typeof(HotDrinkMachine).Assembly.GetTypes()) {
    // with this we can check the implemented factories through the interface
    if (typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface) {
     factories.Add(Tuple.Create(t.Name.Replace("Factory", String.Empty), (IHotDrinkFactory)Activator.CreateInstance(t)));
    }
   }
  }
  // on here we make an interactive interface for the user to select the drink
  public IHotDrink MakeDrink() {
   WriteLine("Available drinks:");
   for(var index = 0; index < factories.Count; index++) {
    var tuple = factories[index];
    WriteLine($"{index}: {tuple.Item1}"); 
   }
   while (true) {
    string s;
    if ((s = Console.ReadLine()) != null && int.TryParse(s, out int i) && i >= 0 && i < factories.Count) {
     Write("Specify amount: ");
     s = ReadLine();
     if (s != null && int.TryParse(s, out int amount) && amount > 0) {
      return factories[i].Item2.Prepare(amount);
     }
    }
    WriteLine("Incorrect input, try again!");
   }
  }
 }

 public class Main
 {
  public static void Run()
  {
   var machine = new HotDrinkMachine();
   var drink = machine.MakeDrink();
   drink.Consume();
  }
 }
}