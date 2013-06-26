using System;
using Given.Common;
using Given.Example;

namespace Given.NUnit.Example
{
    [Story(AsA = "car manufacturer",
        IWant = "a factory that makes the right cars",
        SoThat = "I can make money")]
    public class using_a_car_factory_to_make_a_ford : Scenario
    {
        static readonly CarFactory CarFactory = Context.Given<CarFactory>("a car factory");
        static readonly Tuple<CarFactory,Car> stuff = Context.Given<CarFactory,Car>("a car factory and a ford");
        static readonly Tuple<CarFactory, CarFactory> stuff2 = Context.Given<CarFactory, CarFactory>("a couple of car factories");
        static readonly Tuple<CarFactory, Car,Car> stuff3 = Context.Given<CarFactory, Car,Car>("a car factory and a ford and a toyota");

        public using_a_car_factory_to_make_a_ford()
        {
            Context.Given("a car factory and a ford");
        }

        static Car _car;

        when building_a_ford = () =>
                                   {
                                       Context.Given("a void thing");
                                       _car = CarFactory.Make(CarType.Ford);
                                   };

        [then]
        public void it_should_create_a_car()
        {
            _car.ShouldNotBeNull();
        }

        [then]
        public void it_should_be_the_right_type_of_car()
        {
            _car.Type.ShouldEqual(CarType.Ford);
        }
    }
}