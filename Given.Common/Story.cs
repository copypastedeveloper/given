using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Given.Common
{
    public class Story : IEquatable<Story>
    {
        readonly StoryAttribute _storyAttribute;
        readonly List<TestStateManager> _testManagers;
        
        public Story(StoryAttribute storyAttribute)
        {
            _storyAttribute = storyAttribute;
            
            _testManagers = new List<TestStateManager>();
        }

        public string GetDescription(string format = "{0}{1}{2}")
        {
            return !string.IsNullOrEmpty(_storyAttribute.AsA) ? string.Format(format, _storyAttribute.AsA, _storyAttribute.IWant, _storyAttribute.SoThat) : string.Empty;
        }

        public List<TestResult> Tests
        {
            get
            {
                //this is for MSTest.  Each 'then' in MSTest gets its own TestStateManager, here we combine all of them back together
                var consolidatedTests = _testManagers
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

                return consolidatedTests.Select(x => new TestResult(x.Givens.Select(y => y.Value), x.Whens.Select(y => y.Value), x.Thens.Select(y => y.Value), x.TestType)).ToList();
            }
        }

        public void AddTestStateManager(TestStateManager testStateManager)
        {
            _testManagers.Add(testStateManager);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Story)obj);
        }

        public bool Equals(Story other)
        {
            return string.Equals(GetDescription(), other.GetDescription());
        }

        public override int GetHashCode()
        {
            return (string.IsNullOrEmpty(GetDescription()) ? GetDescription().GetHashCode() : 0);
        }
    }
}