using Given.Common;

namespace Given.NUnit.Example
{
    public class when_testing_nonsense : Specification
    {
        given some_nonsense = () => { };
        when doing_nothing = () => { };
        [then]
        public void nothing_should_happen()
        {
            1.ShouldEqual(2);
        }
    }
}