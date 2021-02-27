using static System.Console;

namespace Prototype.DeepCopyInterface
{
 //the IClonable from .net is ambiguous because you don't know if the copy will
 //be deep or shalow, usually being the later which is not what we want
 //so we can define our own Prototype interface clearly statiting that it's
 //a deep copy on its method name
 public interface IPrototype<T>
 {
  T DeepCopy();
 }
 public class Person : IPrototype<Person>
 {
  public Person(string[] names, Address address)
  {
   Names = names;
   Address = address;
  }
  public string[] Names;
  public Address Address;
  public override string ToString()
  {
   return $"{nameof(Names)}: {string.Join(' ', Names)}, {nameof(Address)}: {Address}";
  }
  public Person DeepCopy()
  {
   //then we can return a new instance calling the same deep copy on all its children
   //the problem here is that we have traverse all the tree implementing the interface
   //which if there is a deep hierarchy of objects can be a daunting task
   return new Person(Names, Address.DeepCopy());
  }
 }
 public class Address : IPrototype<Address>
 {
  public Address(string streetName, int houseNumber)
  {
   this.StreetName = streetName;
   this.HouseNumber = houseNumber;
  }
  public string StreetName;
  public int HouseNumber;
  public override string ToString()
  {
   return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
  }
  public Address DeepCopy()
  {
   return new Address(this.StreetName, this.HouseNumber);
  }
 }
 public class Main
 {
  public static void Run()
  {
   var john = new Person(new string[] { "John", "Smith" }, new Address("London Road", 123));
   var jane = john.DeepCopy();
   jane.Address.StreetName = "Main Road";
   WriteLine(john);
   WriteLine(jane);
  }
 }
}