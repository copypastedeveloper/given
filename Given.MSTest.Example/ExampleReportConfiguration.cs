using System;
using Given.Common;

namespace Given.MSTest.Example
{
    public class ExampleReportConfiguration : IReportConfiguration
    {
        public object TestRunId { get { return Guid.NewGuid(); } }
        public string AssemblyHeader { get { return "MSTest Example"; } }
        public string TestResultDirectory { get
        {
            var location = GetType().Assembly.CodeBase;
            return new Uri(location.Substring(0, location.LastIndexOf("bin", StringComparison.Ordinal))).AbsolutePath;
        }
        }
    }
}