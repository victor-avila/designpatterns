using System;

namespace DesignPatterns
{

 //avoid building interfaces that are too large
 //so that nobody have to implement functions that they don't actually need
 //break them appart into multiple smaller interfaces

 public class Document
 {
 }

 public interface IMachine
 {
  void Print(Document d);
  void Scan(Document d);
  void Fax(Document d);
 }
 public class MultiFunctionPrinter : IMachine
 {
  public void Print(Document d)
  {
   //
  }
  public void Scan(Document d)
  {
   //
  }
  public void Fax(Document d)
  {
   //
  }
 }

 public class OldFashionPrinter : IMachine
 {
  public void Print(Document d)
  {
   //
  }
  //here we have methods that aren't needed and we have to 
  //decide if those throw an exception or just do nothing
  public void Scan(Document d)
  {
   throw new NotImplementedException();
  }
  public void Fax(Document d)
  {
   throw new NotImplementedException();
  }
 }

 public interface IPrinter
 {
  void Print(Document d);
 }
 public interface IScanner
 {
  void Scan(Document d);
 }
 public interface IMultiFunctionDevice : IPrinter, IScanner
 {
 }
 //decorator pattern
 public class MultiFunctionMachine : IMultiFunctionDevice
 {
  private IPrinter printer;
  private IScanner scanner;
  public MultiFunctionMachine(IPrinter printer, IScanner scanner)
  {
   this.printer = printer;
   this.scanner = scanner;
  }
  //here we delagate each action to its respective interface
  public void Print(Document d)
  {
   this.printer.Print(d);
  }
  public void Scan(Document d)
  {
   this.scanner.Scan(d);
  }
 }

 public class InterfaceSegregationPattern
 {
  public static void Run()
  {

  }
 }
}