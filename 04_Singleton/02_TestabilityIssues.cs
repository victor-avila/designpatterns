using static System.Console;
using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework;
namespace Singleton.TestabilityIssues
{
 public interface IDatabase
 {
  int GetPopulation(string name);
 }
 public class SingletonDatabase : IDatabase
 {
  public Dictionary<string, int> capitals;

  //this new property is to be able to test
  //that there's only one instance created
  private static int instanceCount; // 0 by default
  public static int Count => instanceCount;

  private SingletonDatabase()
  {
   //here we register the number of instances created or 
   //how many times the constructor is actually called
   instanceCount++;
   WriteLine("Initializing Database");
   capitals = FileToDictionary("04_Singleton/capitals.txt");
  }
  public int GetPopulation(string name)
  {
   return capitals[name];
  }
  private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());
  public static SingletonDatabase Instance => instance.Value;
  private Dictionary<string, int> FileToDictionary(string fileName)
  {
   var dic = new Dictionary<string, int>();
   string[] text = File.ReadAllLines(Path.Combine(new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, fileName));
   for (int i = 0; i < text.Length; i = i + 2)
   {
    dic.Add(text[i].Trim(), int.Parse(text[i + 1].Trim()));
   }
   return dic;
  }
 }

 //component to consume the singleton
 //calcultes the total population given a list of cities
 public class SingletonRecordFinder {
  public int GetTotalPopulation(IEnumerable<string> names) {
   int result = 0;
   foreach(var name in names) {
    result += SingletonDatabase.Instance.GetPopulation(name);
   }
   return result;
  }
 }

 //to run tests on VS code it's necessary to install the NuGet Package Manager
 //from jmrog, then with CMD + SHIFT + P call NuGet Package Manager: Add Package
 //then add NUnit, NUnit3Adapter and Microsoft.NET.Test.Sdk
 //then edit the csproj and add the following key
 // <GenerateProgramFile>false</GenerateProgramFile>
 //</PropertyGroup>
 //that is because the test sdk creates another entry point for a console app
 //but this one is already a console with a main entry
 //then clear and build the project with donet build and dotnet build
 //then call the specific test through the dll with the following command
 //dotnet test "bin/Debug/net5.0/DesignPatterns.dll" --filter "Singleton.TestabilityIssues.SingletonTests.IsSingletonTest"
 //the filter corresponds to --filter "Namespace.ClassName.MethodName"
 [TestFixture]
 public class SingletonTests {
  //to make sure that the number of instances we have is 1 and
  //any new instance will be refered to the same object
  [Test]
  public void IsSingletonTest() {
   var db = SingletonDatabase.Instance;
   var db2 = SingletonDatabase.Instance;
   Assert.That(db, Is.SameAs(db2));
   Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
  }

  //here instead of testing the db directly we'll test somobody that uses the db.
  //the test passes, but the problem with that is that we are using a live database 
  //to build the tests which is not good practice on unit testing, we should be able
  //to provide (mock) the db so we know beforehand the expected results, on this case
  //we had to manually lookup in the db (or file) to get the numbers of the populations to
  //use them on the test validation, another problem is that the state of a live database can
  //change easily damaging the tests and also is a problem with the resources needed to 
  //use a db on testing, which will make it unnecessarily slower
  //you really want to fake the entire database, but it's not possible because the record finer
  //on this case has a hard coded reference to the instance, that's the problem with singleton, that
  //you cannot substitute the instance with something else on this case for testing purposes
  [Test]
  public void SingletonTotalPopulationTest() {
   var rf = new SingletonRecordFinder();
   var names = new[] {"Seoul", "Mexico City"};
   int tp = rf.GetTotalPopulation(names);
   Assert.That(tp, Is.EqualTo(17500000 + 17400000));
  }
 }

 public class Main
 {
  public interface IDatabase
  {
   int GetPopulation(string name);
  }
  public static void Run()
  {
   var db = SingletonDatabase.Instance;
   WriteLine(db.GetPopulation("Tokyo"));
   WriteLine("TestabilityIssues");
  }
 }
}