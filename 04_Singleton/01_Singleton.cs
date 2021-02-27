using static System.Console;
using System.Collections.Generic;
using System.IO;
using System;
//here we are simulating a database connection, arguing
//that we only need one per execution, on this case the source
//isn't changin so there's no point on creating it every time it's used
namespace Singleton.Singleton
{
 public interface IDatabase
 {
  int GetPopulation(string name);
 }
 public class SingletonDatabase : IDatabase
 {
  public Dictionary<string, int> capitals;
  //you wan't to prevent anyone (developers and anyone consuming the library externally)
  //from making more than one singleton database, then the constructor is made private
  private SingletonDatabase()
  {
   WriteLine("Initializing Database");
   //must include on csproj:
   // <ItemGroup Condition="'$(Configuration)' == 'Debug'">
   //  <None Update="04_Singleton\capitals.txt" CopyToOutputDirectory="PreserveNewest" />
   // </ItemGroup>
   //</Project>
   capitals = FileToDictionary("04_Singleton/capitals.txt");
  }
  public int GetPopulation(string name)
  {
   return capitals[name];
  }
  //This is the basic implementation
  // private static SingletonDatabase instance = new SingletonDatabase();
  // public static SingletonDatabase Instance => instance;
  //But if we wish to make it lazy:
  //on this case database is initialized only if SingletonDatabase.Instance is called, this
  //is because sometimes the resource isn't called on the execution and there's no need to pay
  //the price always
  private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());
  public static SingletonDatabase Instance => instance.Value;
  private Dictionary<string, int> FileToDictionary(string fileName)
  {
   var dic = new Dictionary<string, int>();
   string[] text = File.ReadAllLines(fileName);
   for (int i = 0; i < text.Length; i = i + 2)
   {
    dic.Add(text[i].Trim(), int.Parse(text[i + 1].Trim()));
   }
   return dic;
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
  }
 }
}