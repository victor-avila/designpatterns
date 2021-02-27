using static System.Console;
using System.Collections.Generic;
using System.IO;
using System;
using NUnit.Framework;
using Autofac;
namespace Singleton.DependencyInjection
{
 public interface IDatabase
 {
  int GetPopulation(string name);
 }

 //to avoid de testability issues we can define the database to use
 //as a dependancy instead of hardcoded on a static property on the class
 //this way we can fed the database into the constructor and this way
 //we can also mock the database and pass it thorugh injection
 public class ConfigurableRecordFinder
 {
  //here the database must be provider on the constructor
  IDatabase database;
  public ConfigurableRecordFinder(IDatabase database)
  {
   this.database = database;
  }
  public int GetTotalPopulation(IEnumerable<string> names)
  {
   int result = 0;
   foreach (var name in names)
   {
    result += database.GetPopulation(name);
   }
   return result;
  }
 }

 //mock database to use in tests
 public class DummyDatabase : IDatabase
 {
  public int GetPopulation(string name)
  {
   return new Dictionary<string, int>
   {
    ["alpha"] = 1,
    ["beta"] = 2,
    ["gamma"] = 3
   }[name];
  }
 }

 //real database, here we are not defining a singleton
 //but a normal class, and the behaviour of a singleton will
 //be managed by the dependency injection container framework (Autofac)
 public class OrdinaryDatabase : IDatabase
 {
  public Dictionary<string, int> capitals;

  public OrdinaryDatabase()
  {
   WriteLine("Initializing Database");
   capitals = FileToDictionary("04_Singleton/capitals.txt");
  }
  public int GetPopulation(string name)
  {
   return capitals[name];
  }
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

 [TestFixture]
 public class SingletonTests
 {

  //dotnet test "bin/Debug/net5.0/DesignPatterns.dll" --filter "Singleton.DependencyInjection.SingletonTests.ConfigurablePopulationTest"
  [Test]
  public void ConfigurablePopulationTest()
  {
   //here we just pass the database on the constructor when
   //calling the classes using it, on this case with a mock for
   //testing
   var rf = new ConfigurableRecordFinder(new DummyDatabase());
   var names = new[] { "alpha", "gamma" };
   int tp = rf.GetTotalPopulation(names);
   Assert.That(tp, Is.EqualTo(4));
  }
 }

 public class Main
 {
  public static void Run()
  {
   //in the real world instead of building a singleton yourself you delegate
   //to have something in singleton form to a dependency injection container
   //on this case we are using Autofac
   var cb = new ContainerBuilder();
   //here OrdinaryDatabase is not a singleton but but it gets treated
   //as a singleton by the entire application thanks to the dependency
   //injection container, for autofac calling .SingleInstance()
   //on the following call chain we tell autofac that whenever someone
   //asks for an IDatabase you are going to give them an OrdinaryDatabase
   //but as a singleton
   cb.RegisterType<OrdinaryDatabase>()
    .As<IDatabase>()
    .SingleInstance();
   cb.RegisterType<ConfigurableRecordFinder>();
   using (var c = cb.Build())
   {
    //this is the way to instantiate a class object using
    //the dependency container
    var rf = c.Resolve<ConfigurableRecordFinder>();
    var names = new[] { "Osaka", "Mumbai" };
    int tp = rf.GetTotalPopulation(names);
    WriteLine(tp);
   }
  }
 }
}