using System;
using Given.Common;
using Given.Example;

namespace Given.NUnit.Example
{
    public class ContextProvider : IContextProvider
    {
        public void SetupContext()
        {
            Context.Register("a car factory")
                   .As(() => new CarFactory())
                   .WithCleanup(factory => Console.WriteLine("Blew up the factory!"));
            
            Context.Register("a void thing")
                   .As(() => { })
                   .WithCleanup(() => Console.WriteLine("this is the nothing thing!"));
            
            Context.Register("a couple of car factories")
                   .As<CarFactory,CarFactory>(() => new Tuple<CarFactory,CarFactory>(new CarFactory(), new CarFactory()))
                   .WithCleanup((factory, carFactory) => Console.WriteLine("Blew up the factories!"));

            Context.Register("a car factory and a ford")
                   .As<CarFactory,Car>(() => new Tuple<CarFactory,Car>(new CarFactory(), new CarFactory().Make(CarType.Ford)))
                   .WithCleanup((factory, ford) => Console.WriteLine("Blew up the factory and the {0}!",ford.Type));
        }
    }
}