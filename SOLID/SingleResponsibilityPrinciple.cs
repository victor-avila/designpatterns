using System;
using System.Collections.Generic;
using System.IO;

namespace DesignPatterns
{
 public class Journal
 {
  private readonly List<string> entries = new List<string>();

  private static int count = 0;

  public int AddEntry(string text)
  {
   entries.Add($"{++count}: {text}");
   return count; //memento pattern
  }
  public void RemoveEntry(int index)
  {
   entries.RemoveAt(index);
  }
  public override string ToString()
  {
   return String.Join(Environment.NewLine, entries);
  }
  //The following methods should go on a separate class (Persistence)
  public void Save(string filename)
  {
   System.IO.File.WriteAllText(filename, ToString());
  }
  public static Journal Load(string filename)
  {
   //
   return new Journal();
  }
  public static void Load(Uri uri)
  {
   //
  }
 }

 public class Persistence
 {
  public void SaveToFile(Journal j, string filename, bool overwrite = false)
  {
   if (overwrite || !File.Exists(filename))
   {
    File.WriteAllText(filename, j.ToString());
   }
  }
 }

 public static class SingleResponsibilityPrinciple
 {
  public static void Run()
  {
   var j = new Journal();
   j.AddEntry("I cried today");
   j.AddEntry("I ate a bug");
   Console.WriteLine(j);

   var p = new Persistence();
   var filename = @"journal.txt";
   p.SaveToFile(j, filename, true);
  }
 }

}
