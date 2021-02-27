using static System.Console;
//.net offers the interface IClonable, where we have to implement the
//method clone, but it is very inconvenient as it does not tell if 
//it's a deep or shallow copy, usually being the first which does not
//works for prototyping, also it returns an object type, probably
//because it was created before generics and we have to cast every time
//to the original object

//one better approach although not very common in .net is the use
//of copy constructors on which we use a constructor overload returing
//the same type and there we implement the deep copy, it's a concept
//that comes from c++, but again not very popular on .net
namespace Prototype.CopyConstructor
{
 public class Person
 {
  public Person(string[] names, Address address)
  {
   Names = names;
   Address = address;
  }
  public Person(Person other)
  {
   Names = other.Names;
   // it's important to implement and call all the copy constructors 
   // from the reference type properties
   Address = new Address(other.Address);
  }
  public string[] Names;
  public Address Address;
  public override string ToString()
  {
   return $"{nameof(Names)}: {string.Join(' ', Names)}, {nameof(Address)}: {Address}";
  }
 }
 public class Address
 {
  public Address(string streetName, int houseNumber)
  {
   this.StreetName = streetName;
   this.HouseNumber = houseNumber;
  }
  // we have to implement the copy constructor all the
  // way down the tree
  public Address(Address other)
  {
   StreetName = other.StreetName;
   HouseNumber = other.HouseNumber;
  }
  public string StreetName;
  public int HouseNumber;
  public override string ToString()
  {
   return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
  }
 }
 public class Main
 {
  public static void Run()
  {
   var john = new Person(new string[] { "John", "Smith" }, new Address("London Road", 123));
   var jane = new Person(john);
   jane.Names = new string[] { "Jane", "Austin" };
   // if cloned correctly (deep copy) changing the reference type properties should not
   // affect the original john object
   jane.Address.StreetName = "Main Road";
   jane.Address.HouseNumber = 321;
   WriteLine(john);
   WriteLine(jane);
  }
 }
}