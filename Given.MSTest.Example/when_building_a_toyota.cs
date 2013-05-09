using Given.Common;
using Given.Example;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Given.MSTest.Example
{
    [TestClass]
    public class when_building_a_toyota : Scenario
    {
        static CarFactory _factory;
        static Car _car;

        given a_car_factory = () =>
                                  {
                                      _factory = new CarFactory();
                                  };

        when building_a_toyota = () => _car = _factory.Make(CarType.Toyota);

        [TestMethod]
        public void it_should_create_a_car()
        {
            //the then method allows us to log exceptions.
            //if that isn't important to you, you can leave it out.
            then(() =>_car.ShouldNotBeNull());
        }

        [TestMethod]
        public void it_should_be_the_right_type_of_car()
        {
            then(() => _car.Type.ShouldEqual(CarType.Toyota));
        }

        [TestMethod]
        public void it_should_be_the_wrong_type_of_car()
        {
            then(() => _car.Type.ShouldEqual(CarType.Ford));
        }
    }
}