using static System.Console;

//one question with singleton could be why not just to create 
//a static class with static members, which is a bad idea because
//that way we wouldn't even have constructors, then we can't use
//independency injection for example, so no testability
//this variation called monostate which also tries to use static
//members in a really bizarre way
namespace Singleton.Monostate
{
 //here wi'll allow the state (members) of the ceo to be
 //static but being exposed in a non-static way
 public class CEO
 {
  private static string name;
  private static int age;
  //we make the properties accesing to the static internal
  //attributes but the properties will not be static
  public string Name
  {
   get => name;
   set => name = value;
  }
  public int Age
  {
   get => age;
   set => age = value;
  }
  public override string ToString()
  {
   return $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}";
  }
 }

 public class Main
 {
  public static void Run()
  {
   //the client will be able to instantiate the CEO
   //thorugh the constructor
   var ceo = new CEO();
   ceo.Name = "Adam Smith";
   ceo.Age = 55;
   //but any other ceo will refer to the same data
   //because the properties exposed actually map
   //to static fields
   var ceo2 = new CEO();
   //tipically our singleton approach is to prevent people from
   //calling constructors, but in this case we do the opposite
   //we can make as many objects as we want, but whenever you
   //actually use the properties on the they still map to static fiels
   //and all share the same data
   //again, this approach is not recommended
   WriteLine(ceo2);
   //Name: Adam Smith, Age: 55
  }
 }
}