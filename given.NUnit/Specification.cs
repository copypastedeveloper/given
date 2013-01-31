using Given.Common;
using NUnit.Framework;

namespace Given.NUnit
{
    [TestFixture]
    public abstract class Specification
    {
        [TestFixtureSetUp]
        public void TestSetup()
        {
            var initializer = new TestInitializer(this, true);

            initializer.ProcessGiven();
            initializer.ProcessWhen();
            initializer.ProcessThen();
        }
    }
}