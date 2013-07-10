using System;
using System.Linq;
using System.Reflection;
using Given.Common;
using NUnit.Framework;
using TestState = Given.Common.TestState;

namespace Given.NUnit
{
    [TestFixture]
    public abstract class Scenario
    {
        TestStateManager _testStateManager;

        [TestFixtureSetUp]
        public void Setup()
        {
            _testStateManager = new TestStateManager(this);
            var initializer = new TestInitializer(this,_testStateManager);

            //todo: there needs to be a common way to ignore setup for scenarios where all tests are ignored.  this sucks.
            if (!GetType().GetMethods().Any(y => new DefaultTestRunnerConfiguration().ThenIdentificationMethod(y) &&
                                                 !y.GetCustomAttributes(typeof (IgnoreAttribute), true).Any()))
            {
                return;
            }
            
            initializer.ProcessDelegates();
            _testStateManager.WriteSpecification();
        }

        [TearDown]
        public void RecordState()
        {
            var context = TestContext.CurrentContext;
            TestState state;

            TestStatus status;
            try
            {
                status = context.Result.Status;
            }
            catch (Exception)
            {
                status = TestStatus.Inconclusive;
            }

            switch (status)
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

            var message = GetTestExecutionMessage();

            try
            {
                _testStateManager.SetThenState(context.Test.Name, state, message == null ? string.Empty : message.ToString());
            }
            catch (Exception)
            {
            }
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _testStateManager.Cleanup();
        }

        static object GetTestExecutionMessage()
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var nunitCore = assemblies.First(x => x.GetName().Name == "nunit.core");
                var nunitCoreInterfaces = assemblies.First(x => x.GetName().Name == "nunit.core.interfaces");
                var testExecutionContextType = nunitCore.GetType("NUnit.Core.TestExecutionContext");
                var testResultType = nunitCoreInterfaces.GetType("NUnit.Core.TestResult");
                var currentContextMethod = testExecutionContextType.GetProperty("CurrentContext", BindingFlags.Public | BindingFlags.Static).GetGetMethod();
                var testResultMethod = testExecutionContextType.GetProperty("CurrentResult", BindingFlags.Public | BindingFlags.Instance).GetGetMethod();
                var messageMethod = testResultType.GetProperty("Message", BindingFlags.Public | BindingFlags.Instance).GetGetMethod();

                var testExecutionContext = currentContextMethod.Invoke(null, null);
                var testResult = testResultMethod.Invoke(testExecutionContext, null);
                var message = messageMethod.Invoke(testResult, null);
                return message;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}