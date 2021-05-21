using static System.Console;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

// to connect neurons and also define layers reusing conection method

namespace Composite.NeuralNetworks
{
 // we add an extension method to IEnumerable so that its part of
 // on the Neuron class once implemented and also on the NeuronLayer class
 // as it's implementing Collection that implements it underneath
 public static class ExtensionMethods
 {
  // this method connects 2 groups of neurons, the single neuron will be a collection
  // of 1 as it's implementing IEnumerable as well
  public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
  {
   if (ReferenceEquals(self, other)) return;
   foreach (var from in self)
   {
    foreach (var to in other)
    {
     from.Out.Add(to);
     to.In.Add(from);
    }
   }
  }
 }

 public class Neuron : IEnumerable<Neuron>
 {
  public float Value;
  public List<Neuron> In, Out;

  public Neuron()
  {
   In = new List<Neuron>();
   Out = new List<Neuron>();
  }
  // this method is moved to an extension method to make it
  // availabole to all the kinds of neuron groupings
  // public void ConnectTo(Neuron other)
  // {
  //  Out.Add(other);
  //  other.In.Add(this);
  // }
  // here we only return the current neuron as the
  // one and only containing elmement, this methods
  // are the implementation of the IEnumerable interface
  public IEnumerator<Neuron> GetEnumerator()
  {
   yield return this;
  }
  IEnumerator IEnumerable.GetEnumerator()
  {
   return GetEnumerator();
  }
 }

 // a neuron layer will be just a collection of neurons
 // with this approach we can continue adding other groupings
 // like for example rings and so on and don't have
 // to define a connection method for every new kind of group with
 // all the others, just to implement an IEnumerable
 public class NeuronLayer : Collection<Neuron>
 {

 }

 public class Main
 {
  public static void Run()
  {
   // we want to be able to connect neurons to layers
   // to other layers and also connections between layers,
   // on the case of layers it will create a connection
   // for every item that's part of the group
   var neuron1 = new Neuron();
   var neuron2 = new Neuron();

   neuron1.ConnectTo(neuron2);

   var layer1 = new NeuronLayer();
   var layer2 = new NeuronLayer();

   neuron1.ConnectTo(layer1);
   layer1.ConnectTo(layer2);
  }
 }
}