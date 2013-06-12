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
        static Lazy<CarFactory> _carFactory = Context.Given<CarFactory>("a car factory");
        static Car _car;

        when building_a_ford = () => _car = _carFactory.Value.Make(CarType.Ford);

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