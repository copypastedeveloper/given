namespace Given.Example
{
    public class CarFactory
    {
        public Car Make(CarType carType)
        {
            return new Car {Type = carType};
        }
    }
}