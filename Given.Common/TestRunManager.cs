using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Given.Common
{
    internal static class TestRunManager
    {
        static TestRun _testRun;
        static readonly IEnumerable<Type> Processors;

        static TestRunManager()
        {
            Processors =
                Assembly.GetAssembly(typeof (TestRunManager))
                        .GetTypes()
                        .Where(x => typeof (ITestResultProcessor).IsAssignableFrom(x) &&
                                    x.IsAbstract == false &&
                                    x.IsGenericTypeDefinition == false &&
                                    x.IsInterface == false);

            AppDomain.CurrentDomain.DomainUnload += Unload;
        }

        static void Unload(object sender, EventArgs e)
        {
            var testRunResults = CurrentTestRun.GetTestRunResults();
            
            foreach (var processor in Processors)
            {
                ((ITestResultProcessor) Activator.CreateInstance(processor)).Process(testRunResults, Guid.NewGuid());
            }
        }

        public static TestRun CurrentTestRun
        {
            get { return _testRun ?? (_testRun = new TestRun()); }
        }
    }
}