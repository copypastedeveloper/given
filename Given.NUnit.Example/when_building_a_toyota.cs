using System;
using Given.Common;
using Given.Example;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Given.NUnit.Example
{
    [Story(AsA = "car manufacturer",
         IWant = "a factory that makes the right cars",
         SoThat = "I can make money"), TestFixture]
    public class building_a_toyota : Scenario
    {
        static CarFactory _factory;
        static Car _car;

        before testing = () => Console.WriteLine("this is executed before the given");

        given a_car_factory = () =>
        {
            _factory = new CarFactory();
        };

        when building_a_car = () => _car = _factory.Make(CarType.Toyota);

        [then]
        public void it_should_create_a_car()
        {
            _car.ShouldNotBeNull();
        }

        [then]
        public void it_should_be_the_right_type_of_car()
        {
            _car.Type.ShouldEqual(CarType.Toyota);
        }

        after test_completion = () => Console.WriteLine("executed the tear down");
    }
}