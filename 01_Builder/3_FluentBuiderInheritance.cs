// here we have a base fluent builder, but will be using inheritance
// to extend that builder, we should be able to chain
// the methods from the different child builders in one line

// there is no easy way to mitigate the inheritance of fluent interfaces
// but it can be done using recursive generics, a fairly well known approach
using static System.Console;

namespace FluentBuilderInheritance
{
 public class Person
 {
  public string Position;
  public string Name;

  //this is because we can't just initialize a PersonJobBuilder directly
  //(new PersonJobBuilder<???>) as we don't have a type to pass on to the generic argument
  //so we create an internal class inside person to use as that type,
  //this way Person exposes its own builder 
  public class Builder : PersonJobBuilder<Builder> { }
  //this syntax is called expression-bodied member (C#6)
  //just syntax sugar to code a normal getter
  //that is just for getters, for setters this can be don from C#7:
  // public string Name
  // {
  //  get => _name;
  //  set => _name = value;
  // }
  public static Builder New => new Builder();

  public override string ToString()
  {
   return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
  }
 }
 public abstract class PersonBuilder
 {
  //is protected instead of private because will be using inheritance
  protected Person Person = new Person();
  public Person Build()
  {
   return Person;
  }
 }
 //the where condition is to guarantee that the returning type
 //on Called will be PersonBuilder<SELF>, self being for example
 //PersonJobBuilder or PersonInfoBuilder
 public class PersonInfoBuilder<SELF> : PersonBuilder
  where SELF : PersonInfoBuilder<SELF>
 {
  public SELF Called(string name)
  {
   Person.Name = name;
   return (SELF)this;
  }
 }
 //it can be seen that as we inherit more builders 
 //the list type (PersonInfoBuilder<PersonJobBuilder<SELF>>)
 //will grow even more, this is normal
 public class PersonJobBuilder<SELF> : PersonInfoBuilder<PersonJobBuilder<SELF>>
  where SELF : PersonJobBuilder<SELF>
 {
  //basically what we have to do is to propagate the return type from
  //the derived class to the base class, it's done with recursive generics so that 
  //if afterwards we decide to create another class inheriting from this
  //derived class we can continue with the same approach
  public SELF WorksAsA(string position)
  {
   Person.Position = position;
   return (SELF)this;
  }
 }

 public class FluentBuilderInheritance
 {
  public static void Run()
  {
   //here Person.New will always get the most derived kind of type
   //on this case PersonJobBuilder<Person.Builder> which have access
   //to both Called and WorksAsA methods
   var me = Person.New.Called("dmitri").WorksAsA("quant").Build();
   WriteLine(me);
  }
 }
}