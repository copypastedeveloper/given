using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Given.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Given.MSTest
{
    [TestClass]
    public abstract class Scenario
    {
        readonly TestStateManager _testStateManager;
        static readonly ConcurrentDictionary<Type, TestStateManager> AlreadyRanDelegates = new ConcurrentDictionary<Type, TestStateManager>();
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
        protected string Message { get; set; }

        protected Scenario()
        {
            var firstRun = !AlreadyRanDelegates.TryGetValue(GetType(), out _testStateManager);

            if (firstRun)
            {
                _testStateManager = new TestStateManager(this);
                var initializer = new TestInitializer(this, _testStateManager);
                initializer.ProcessDelegates();

                AlreadyRanDelegates.TryAdd(GetType(), _testStateManager);
            }
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

            if (_testStateManager.Thens.All(x => x.Value.State !=TestState.Unknown))
            {
                _testStateManager.Cleanup();
            }
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
