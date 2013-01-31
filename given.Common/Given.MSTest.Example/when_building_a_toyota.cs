using Given.Common;
using Given.Example;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Given.MSTest.Example
{
    [TestClass]
    public class when_building_a_toyota : Specification
    {
        static CarFactory _factory;
        static Car _car;

        given a_car_factory = () =>
                                  {
                                      _factory = new CarFactory();
                                  };

        when building_a_toyota = () => _car = _factory.Make(CarType.Toyota);

        //adding the then attribute is purely beautification for an ms test test.  
        //this will cause it to be logged to the console appropriately and helps for reading consistency.
        [then, TestMethod]
        public void it_should_create_a_car()
        {
            _car.ShouldNotBeNull();
        }

        [then, TestMethod]
        public void it_should_be_the_right_type_of_car()
        {
            _car.Type.ShouldEqual(CarType.Toyota);
        }
    }
}