using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Autofac;
using Autofac.Features.Metadata;
using static System.Console;

// how autofac helps you build adapters and inject them correctly too
namespace Adapter.AdapterInDependencyInjection
{
 public interface ICommand
 {
  void Execute();
 }
 public class SaveCommand : ICommand
 {
  public void Execute()
  {
   WriteLine("Saving a file");
  }
 }
 public class OpenCommand : ICommand
 {
  public void Execute()
  {
   WriteLine("Opening a file");
  }
 }
 public class Button
 {
  private ICommand command;
  private string name;
  public Button(ICommand command, string name)
  {
   this.command = command;
   this.name = name;
  }
  public void Click()
  {
   command.Execute();
  }
  public void PrintMe()
  {
   WriteLine($"I'm a button called {name}");
  }
 }
 public class Editor
 {
  private IEnumerable<Button> buttons;
  public IEnumerable<Button> Buttons => buttons;
  public Editor(IEnumerable<Button> buttons)
  {
   this.buttons = buttons;
  }
  public void ClickAll()
  {
   foreach (var btn in buttons)
   {
    btn.Click();
   }
  }
 }
 public class Main
 {
  public static void Run()
  {
   var b = new ContainerBuilder();
   // the WithMetadata method is added after including the name
   // attribute on Button, it's to send additional info
   // when registering the adapter
   b.RegisterType<SaveCommand>().As<ICommand>().WithMetadata("Name", "Save");
   b.RegisterType<OpenCommand>().As<ICommand>().WithMetadata("Name", "Open");
   // this way only one button will be registered
   //b.RegisterType<Button>();
   // with this one the two buttons will be registered because we are specifying
   // the interface to bring the two buttons from the two interface implementations
   //b.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd));
   // here we have the same but passing some metadata defined before
   b.RegisterAdapter<Meta<ICommand>, Button>(cmd => new Button(cmd.Value, (string)cmd.Metadata["Name"]));
   b.RegisterType<Editor>();
   using (var c = b.Build())
   {
    var editor = c.Resolve<Editor>();
    // this one is just to demosntrate that all the buttons were registered
    //editor.ClickAll();
    foreach (var btn in editor.Buttons)
    {
     // here we demonstrate that the name is passed on through the metadata
     btn.PrintMe();
    }
   }
  }
 }
}