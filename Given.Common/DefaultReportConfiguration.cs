using System;
using System.IO;
using System.Linq;

namespace Given.Common
{
    internal class DefaultReportConfiguration : IReportConfiguration
    {
        public object TestRunId { get { return Guid.NewGuid(); } }
        public string AssemblyHeader { get { return TestRunManager.CurrentTestRun.GetStories().SelectMany(x => x.Tests).First().Type.Assembly.GetName().Name; } }
        public string TestResultDirectory { get { return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName; } }
    }
}