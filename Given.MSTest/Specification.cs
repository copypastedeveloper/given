using System;
using Given.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Given.MSTest
{
    [TestClass]
    public abstract class Specification
    {
        readonly TestInitializer _initializer;
        readonly TestStateManager _testStateManager;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
        protected string Message { get; set; }

        protected Specification()
        {
            _testStateManager = new TestStateManager(this);            
            _initializer = new TestInitializer(this);
            _initializer.ProcessGiven(_testStateManager);
            _initializer.ProcessWhen(_testStateManager);
            _initializer.ProcessThen(_testStateManager);
        }

        [TestInitialize]
        public void Setup()
        {
            _testStateManager.WriteSpecification(TestContext.WriteLine);            
        }

        [TestCleanup]
        public void RecordState()
        {
            TestState state;

            switch (TestContext.CurrentTestOutcome)
            {
                case UnitTestOutcome.Error:
                case UnitTestOutcome.Failed:
                case UnitTestOutcome.Timeout:
                    state = TestState.Failed;
                    break;
                case UnitTestOutcome.Passed:
                    state = TestState.Passed;
                    break;
                case UnitTestOutcome.Inconclusive:
                    state = TestState.Ignored;
                    break;
                default:
                    state = TestState.Unknown;
                    break;
            }
            _testStateManager.SetThenState(TestContext.TestName, state, Message ?? string.Empty);
        }

        protected void then(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                throw;
            }
        }
    }
}
