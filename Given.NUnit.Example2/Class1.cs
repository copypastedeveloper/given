using Given.Common;

namespace Given.NUnit.Example2
{
    public class when_something : Specification
    {
        given your_mom = () => { };
        when doing_her = () => { };
        [then]
        public void it_is_good() {}
    }
}
