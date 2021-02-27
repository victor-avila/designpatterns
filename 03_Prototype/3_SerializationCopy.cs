using static System.Console;
using System.IO;
using System.Xml.Serialization;
namespace Prototype.SerializationCopy
{
 //to avoid having to create a method on every class to deep copy
 //the object, we can use an extension method like here using
 //generics to give any object the option to make a deep copy
 //for the implementation, we will serialize and then deserialize
 //the object to get an exact copy including all the hierarchy
 //that serializer is not limited to the presented here (binary and xml)
 //any other that has the capability to deserialize in an object can work as well
 public static class ExtensionMethods
 {
  //this method requires to add [Serializable] to every class on the
  //hierarchy, unfortunately it is not supported on .net core and
  //some classes used are obsolete, which it's sad because it is the 
  //fastest method
  // public static T DeepCopy<T>(this T self)
  // {
  //  var stream = new MemoryStream();
  //  var formatter = new BinaryFormatter();
  //  formatter.Serialize(stream, self);
  //  stream.Seek(0, SeekOrigin.Begin);
  //  object copy = formatter.Deserialize(stream);
  //  stream.Close();
  //  return (T)copy;
  // }
  // here we use the xml serializer, which only requires
  // that the class using it has a constructor without parameters
  public static T DeepCopy<T>(this T self)
  {
   using (var ms = new MemoryStream())
   {
    var s = new XmlSerializer(typeof(T));
    s.Serialize(ms, self);
    ms.Position = 0;
    return (T)s.Deserialize(ms);
   }
  }
 }
 public class Person
 {
  public Person() { }
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
 }
 public class Address
 {
  //usually there are requiremets that have to be satisfied on the classes
  //using them, for the xml serializer is that all classes should have a 
  //constructor without parameters even if it's empty
  public Address() { }
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
 }
 public class Main
 {
  public static void Run()
  {
   var john = new Person(new string[] { "John", "Smith" }, new Address("London Road", 123));
   //here we call the extension method
   var jane = john.DeepCopy();
   jane.Names[0] = "Jane";
   jane.Address.HouseNumber = 321;
   WriteLine(john);
   WriteLine(jane);
  }
 }
}