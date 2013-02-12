using System;
using System.Collections.Generic;
using System.Linq;

namespace Given.Common
{
    internal static class TestRunManager
    {
        static TestRun _testRun;
        static readonly IEnumerable<Type> Processors;
        static readonly Type ReportConfiguration;

        static TestRunManager()
        {
            var concreteTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                         .SelectMany(assembly => assembly.GetTypes())
                         .Where(x => x.IsAbstract == false &&
                                     x.IsGenericTypeDefinition == false &&
                                     x.IsInterface == false).ToList();

            Processors = concreteTypes.Where(x => typeof(ITestResultProcessor).IsAssignableFrom(x));

            ReportConfiguration = concreteTypes.FirstOrDefault(x => typeof (IReportConfiguration).IsAssignableFrom(x) &&
                                                                    x != typeof (DefaultReportConfiguration));

            ReportConfiguration = ReportConfiguration ?? typeof(DefaultReportConfiguration);
            
            AppDomain.CurrentDomain.DomainUnload += Unload;
        }

        static void Unload(object sender, EventArgs e)
        {
            
            var testRunResults = CurrentTestRun.GetTestRunResults().ToList();
            var config = Activator.CreateInstance(ReportConfiguration);            
            foreach (var processor in Processors)
            {
                ((ITestResultProcessor) Activator.CreateInstance(processor,new[] {config})).Process(testRunResults);
            }
        }

        public static TestRun CurrentTestRun
        {
            get { return _testRun ?? (_testRun = new TestRun()); }
        }
    }
}