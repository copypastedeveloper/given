using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Given.Common
{
    public class HtmlTestProcessor : ITestResultProcessor
    {
        string _page;

        public HtmlTestProcessor()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Given.Common.HtmlReport." + "htmlreport.html"))
            using (var reader = new StreamReader(stream))
            {
                _page = reader.ReadToEnd();
            }
            _page = _page.Replace("{Title}", string.Format("Results for {0}", Assembly.GetExecutingAssembly().GetName().Name));
        }

        public void Process(IEnumerable<TestResult> testResults, Guid testRunId)
        {
            var div = new HtmlGenericControl("div");
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + string.Format("TestResults.{0}.{1}.html", DateTime.Now.ToString("yyy.MM.dd"), testRunId);

            foreach (var result in testResults)
            {
                div.Controls.Add(GetTestResultDiv(result));
            }

            var fileStream = new FileStream(path, FileMode.Append);
            var bWriter = new BinaryWriter(fileStream);
            using (var swriter = new StringWriter())
            {
                var writer = new HtmlTextWriter(swriter);
                div.RenderControl(writer);
                _page = _page.Replace("{Content}", swriter.ToString());
            }
            bWriter.Write(_page);
            bWriter.Close();
        }

        public HtmlGenericControl GetTestResultDiv(TestResult result)
        {
            const string tag = "div";
            var div = new HtmlGenericControl(tag);
            div.Attributes.Add("class", "well");
            var currentPrefix = Text.Given;

            var statusCount = result.Thens.GroupBy(x => x.State).Select(x => new { State = x.Key, Count = x.Count() });

            foreach (var status in statusCount)
            {
                var span = new HtmlGenericControl("span");
                string badgeType;

                switch (status.State)
                {
                    case TestState.Failed:
                        badgeType = "error";
                        break;
                    case TestState.Passed:
                        badgeType = "success";
                        break;
                    case TestState.Ignored:
                        badgeType = "info";
                        break;
                    default:
                        badgeType = "warning";
                        break;
                }

                span.Attributes.Add("class", string.Format("badge badge-{0}", badgeType));
                span.InnerText = status.Count.ToString();
                div.Controls.Add(span);
            }

            foreach (var value in result.Givens)
            {
                var control = new HtmlGenericControl(tag)
                {
                    InnerText = String.Format(Text.Print, currentPrefix, value.Replace("_", " "))
                };
                div.Controls.Add(control);
                currentPrefix = Text.And;
            }

            currentPrefix = Text.When;
            foreach (var value in result.Whens)
            {
                var control = new HtmlGenericControl(tag)
                {
                    InnerText = String.Format(Text.Print, currentPrefix, value.Replace("_", " "))
                };
                div.Controls.Add(control);
                currentPrefix = Text.And;
            }

            currentPrefix = Text.Then;
            foreach (var statedThen in result.Thens)
            {
                var control = new HtmlGenericControl(tag)
                {
                    InnerText = String.Format(Text.Print, currentPrefix, statedThen.Name.Replace("_", " "))
                };
                control.Attributes["class"] = statedThen.State.ToString();
                div.Controls.Add(control);
                currentPrefix = Text.And;
            }

            return div;
        }
    }
}