using System;
using Given.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Given.MSTest
{
    public abstract class Specification
    {
        readonly TestInitializer _initializer;

        protected Specification()
        {
            _initializer = new TestInitializer(this, true);
        }

        [TestInitialize]
        public void Setup()
        {
            _initializer.ProcessGiven();
            _initializer.ProcessWhen();
            _initializer.ProcessThen();
        }
    }
}
