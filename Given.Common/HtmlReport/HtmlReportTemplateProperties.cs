using System.Collections.Generic;

namespace Given.Common.HtmlReport
{
    public partial class HtmlReportTemplate
    {
        protected IReportConfiguration ReportConfiguration { get; set; }
        protected IEnumerable<TestResult> TestResults { get; set; }
        protected string GetReportHead()
        {
            return new HtmlReportHead().TransformText();
        }
    }
}