using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Builder.FunctionalBuilder
{
 public class Person
 {
  public string Name, Position;
  public override string ToString()
  {
   return $"Name: {Name}, Position: {Position}";
  }
 }
 //sealed restricts the class to inherit from it
 //only can be extended with extension methods
 //instead of modifying the person in place inside
 //the builder we are going to preserve a list
 // of mutating functions wich affect the person
 public sealed class PersonBuilder
 {
  private readonly List<Func<Person, Person>> actions = new List<Func<Person, Person>>();
  //we take an action and turned it into a func, to preserve fluent interface
  //because at some point we need to use the aggregate interface to apply functions
  //one after another
  private PersonBuilder AddAction(Action<Person> action)
  {
   actions.Add(p => { action(p); return p; });
   return this;
  }
  //to expose the private AddAction
  public PersonBuilder Do(Action<Person> action) => AddAction(action);
  //this is the method that will provide an option to the builder to fill the person name
  public PersonBuilder Called(string name) => Do(p => p.Name = name);
  //here we secuentially invoke all the actions defined
  public Person Build() => actions.Aggregate(new Person(), (p, f) => f(p));
 }
 public static class PersonBuilderExtensions
 {
  //here we add a method to the builder using Do, the same as Called internally did
  public static PersonBuilder WorksAs
   (this PersonBuilder builder, string position) => builder.Do(p => p.Position = position);
 }
 public class FunctionalBuilder
 {
  public static void Run()
  {
   var person = new PersonBuilder()
    .Called("Sarah")
    .WorksAs("developer")
    .Build();
   WriteLine(person);
  }
 }
}