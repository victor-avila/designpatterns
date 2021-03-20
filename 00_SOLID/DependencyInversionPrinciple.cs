using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns
{
 public enum Relationship
 {
  Parent,
  Child,
  Sibling
 }
 public class Person
 {
  public string Name;
 }
 public interface IRelationshipBrowser
 {
  IEnumerable<Person> FindAllChildrenOf(string name);
 }
 public class Relationships : IRelationshipBrowser
 {
  private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();
  public void AddParentAndChild(Person parent, Person child)
  {
   relations.Add((parent, Relationship.Parent, child));
   relations.Add((child, Relationship.Child, parent));
  }
  public List<(Person, Relationship, Person)> Relations => relations;
  public IEnumerable<Person> FindAllChildrenOf(string name)
  {
   return relations
    .Where(x => x.Item1.Name == "John" && x.Item2 == Relationship.Parent)
    .Select(r => r.Item3);
  }
 }
 public class DependencyInversionPrinciple
 {
  public static void Run()
  {
   //high level parts of the system should not depend on low level parts of the
   //system directly, instead they should depend on some kind of absatraction

   var parent = new Person { Name = "John" };
   var child1 = new Person { Name = "Chris" };
   var child2 = new Person { Name = "Mary" };
   var relationships = new Relationships();
   relationships.AddParentAndChild(parent, child1);
   relationships.AddParentAndChild(parent, child2);
   Research2(relationships);
  }

  //the problem with this approach is that we are accesing the Relationships
  //low level data store directly exposing its internal list as public
  //then that class cannot change its mind about how to store data, for example if
  //it wants to start using a dictionary instead of tuples or remove duplication
  public static void Research(Relationships relationships)
  {
   var relations = relationships.Relations;
   foreach (var r in relations.Where(x => x.Item1.Name == "John" && x.Item2 == Relationship.Parent))
   {
    Console.WriteLine($"John has a child called {r.Item3.Name}");
   }
  }
  public static void Research2(IRelationshipBrowser browser)
  {
   foreach (var r in browser.FindAllChildrenOf("John"))
   {
    Console.WriteLine($"John has a child called {r.Name}");
   }
  }
 }
}