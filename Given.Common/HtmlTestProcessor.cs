using System.Collections.Generic;
using System.IO;

namespace Given.Common
{
    public class HtmlTestProcessor : ITestResultProcessor
    {
        readonly IReportConfiguration _reportConfiguration;

        public HtmlTestProcessor(IReportConfiguration reportConfiguration)
        {
            _reportConfiguration = reportConfiguration;
        }

        public void Process(IEnumerable<TestResult> testResults)
        {
            var template = new HtmlReport.HtmlReportTemplate
                               {
                                   ReportConfiguration = _reportConfiguration,
                                   TestResults = testResults
                               };

            var renderedTemplate = template.TransformText();

            using (var outfile = new StreamWriter(_reportConfiguration.TestResultDirectory + string.Format(@"\TestResults.{0}.html", _reportConfiguration.AssemblyHeader),false))
            {
                outfile.Write(renderedTemplate);
            }
        }
    }
}