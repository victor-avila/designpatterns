using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Autofac;
using Autofac.Features.Metadata;
using static System.Console;

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
   b.RegisterType<SaveCommand>().As<ICommand>().WithMetadata("Name", "Save");
   b.RegisterType<OpenCommand>().As<ICommand>().WithMetadata("Name", "Open");
   //b.RegisterType<Button>();
   //b.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd));
   b.RegisterAdapter<Meta<ICommand>, Button>(cmd => new Button(cmd.Value, (string)cmd.Metadata["Name"]));
   b.RegisterType<Editor>();
   using (var c = b.Build())
   {
    var editor = c.Resolve<Editor>();
    //editor.ClickAll();
    foreach (var btn in editor.Buttons)
    {
     btn.PrintMe();
    }
   }
  }
 }
}