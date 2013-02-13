using System.Collections.Generic;

namespace Given.Common.HtmlReport
{
    public partial class HtmlReportTemplate
    {
        public IReportConfiguration ReportConfiguration { get; set; }
        public IEnumerable<TestResult> TestResults { get; set; }
        protected string GetReportHead()
        {
            return new HtmlReportHead().TransformText();
        }
    }
}