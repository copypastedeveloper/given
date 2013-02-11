using System;
using System.Linq;
using Given.Common;
using NUnit.Framework;
using TestState = Given.Common.TestState;

namespace Given.NUnit
{
    [TestFixture]
    public abstract class Specification
    {
        TestStateManager _testStateManager;

        [TestFixtureSetUp]
        public void Setup()
        {
            _testStateManager = new TestStateManager(this);            
            var initializer = new TestInitializer(this);

            initializer.ProcessGiven(_testStateManager);
            initializer.ProcessWhen(_testStateManager);
            initializer.ProcessThen(_testStateManager);

            _testStateManager.WriteSpecification();
        }

        [TearDown]
        public void RecordState()
        {
            var context = TestContext.CurrentContext;
            TestState state;

            switch (context.Result.Status)
            {
                case TestStatus.Failed:
                    state = TestState.Failed;
                    break;
                case TestStatus.Passed:
                    state = TestState.Passed;
                    break;    
                    case TestStatus.Skipped:
                    state = TestState.Ignored;
                    break;
                default:
                    state = TestState.Unknown;
                    break;
            }

            _testStateManager.SetThenState(context.Test.Name, state);
        }
    }
}