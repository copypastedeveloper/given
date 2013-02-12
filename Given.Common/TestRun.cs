using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Given.Common
{
    internal class TestRun
    {
        readonly List<TestStateManager> _tests;
        
        public TestRun()
        {
            _tests = new List<TestStateManager>();
        }

        public void AddTest(TestStateManager testStateManager, Type type)
        {
            _tests.Add(testStateManager);
        }

        public List<TestResult> GetTestRunResults()
        {
            //this is for MSTest.  Each 'then' in MSTest gets its own TestStateManager, here we combine all of them back together
            var consolidatedTests = _tests
                .GroupBy(x => x.TestType, (key, tests) =>
                                                   {
                                                       var aggregatedThens = new PairList<MethodInfo, StatedThen>();
                                                       var testStateManagers = tests.ToList();

                                                       foreach (var test in testStateManagers)
                                                       {
                                                           test.Thens.RemoveAll(x => x.Value.State == TestState.Unknown);
                                                           aggregatedThens.AddRange(test.Thens);
                                                           test.Thens.Clear();
                                                       }

                                                       var first = testStateManagers.First();
                                                       first.Thens.AddRange(aggregatedThens);
                                                       return first;
                                                   });

            return consolidatedTests.Select(x => new TestResult(x.Givens.Select(y => y.Value), x.Whens.Select(y => y.Value), x.Thens.Select(y => y.Value),x.TestType)).ToList();
        }
    }
}