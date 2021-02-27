using System;
using static System.Console;
namespace Builder.FacetedBuilder
{
 public class FacetedBuilder
 {

  //sometimes a single builder isn't enough and we want
  //several builders to build serveral different aspects of a particular object
  //for this we use a facade

  public class Person
  {
   //addresss
   public string StreetAddress, Postcode, City;
   //employment
   public string CompanyName, Position;
   public int AnualIncome;
   public override string ToString()
   {
    return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}, {nameof(City)}: {City}, {nameof(CompanyName)}: {CompanyName}, {nameof(Position)}: {Position}, {nameof(AnualIncome)}: {AnualIncome}";
   }
  }

  //this is a facade to access the other builders
  //also keeps a reference of the builder itself
  public class PersonBuilder //facade
  {
   protected Person person = new Person();
   public PersonJobBuilder Works => new PersonJobBuilder(person);
   public PersonAddressBuilder Lives => new PersonAddressBuilder(person);
   //this is not an ideal programming approach, but this way
   //we can easily return the Person type directly from the builder
   //it's called user-defined conversion operator, to use it
   //we have to specify the target type as the name, pass in the source type
   //and esure that the code returns the correct type, as it's implicit we 
   //can just directly assign PersonBuilder instances to a Person variable
   public static implicit operator Person(PersonBuilder pb) {
    return pb.person;
   }
  }


  public class PersonJobBuilder : PersonBuilder
  {
   //takes a reference to the person we are building
   //the person is inherited from PersonBuilder
   public PersonJobBuilder(Person person)
   {
    this.person = person;
   }
   //fluent api for the builder
   public PersonJobBuilder At(String companyName)
   {
    person.CompanyName = companyName;
    return this;
   }
   public PersonJobBuilder AsA(String position)
   {
    person.Position = position;
    return this;
   }
   public PersonJobBuilder Earning(int amount)
   {
    person.AnualIncome = amount;
    return this;
   }
  }

  public class PersonAddressBuilder : PersonBuilder {
   public PersonAddressBuilder(Person person) {
    this.person = person;
   }
   public PersonAddressBuilder At(string streetAddress) {
    person.StreetAddress = streetAddress;
    return this;
   }
   public PersonAddressBuilder WithPostcode(string postcode) {
    person.Postcode = postcode;
    return this;
   }
   public PersonAddressBuilder In(string city) {
    person.City = city;
    return this;
   }
  }

  public static void Run()
  {
   var pb = new PersonBuilder();
   Person person = pb
    .Lives.At("123 London Road").In("London").WithPostcode("SW12AC")
    .Works.At("Fabrikam").AsA("Engineer").Earning(123000);
   WriteLine(person);
  }
 }
}