using static System.Console;
using Autofac;

// in addition to the adaptaer pattern the autofac container
// also supports the decorator pattern
namespace Decorator.DecoratorInDependencyInjection
{
 public interface IReportingService
 {
  void Report();
 }
 public class ReportingService : IReportingService
 {
  public void Report()
  {
   WriteLine("Here is your report");
  }
 }

 // here we have this decorator wich holds an instance of Report
 // and adds some logs to the report generation
 public class ReportingServiceWithLogging : IReportingService
 {
  IReportingService decorated;
  public ReportingServiceWithLogging(IReportingService decorated)
  {
   this.decorated = decorated;
  }
  public void Report()
  {
   WriteLine("Commencing log...");
   decorated.Report();
   WriteLine("Ending log...");
  }
 }
 public class Main
 {
  public static void Run()
  {
   var b = new ContainerBuilder();
   // usually you register an interface like this
   // b.RegisterType<ReportingService>().As<IReportingService>();
   // but if need ReportingServiceWithLogging as follows
   // b.RegisterType<ReportingServiceWithLogging>().As<IReportingService>();
   // then ReportingServiceWithLogging will inject itself infinitely
   // as it receives IReportingService in the constructor
   // then as we want ReportingServiceWithLogging but injecting a ReportingService
   // but we also want the IReportingService to yield us a ReportingServiceWithLogging
   // we can do it as follows
   b.RegisterType<ReportingService>().Named<IReportingService>("reporting");
   b.RegisterDecorator<IReportingService>((context, service) => new ReportingServiceWithLogging(service), "reporting");
   using (var c = b.Build())
   {
    var r = c.Resolve<IReportingService>();
    r.Report();
   }
  }
 }
}