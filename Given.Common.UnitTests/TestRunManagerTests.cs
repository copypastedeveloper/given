using System;
using NUnit.Framework;

namespace Given.Common.UnitTests
{
	[TestFixture]
	public class TestRunManagerTests
	{
		[Test]
		public void StaticConstructor_OnlyDefaultIReportConfigurationExists_ReportConfigurationIsOfTypeDefaultReportConfiguration()
		{
			Assert.AreEqual(typeof(DefaultReportConfiguration), TestRunManager.ReportConfiguration);
		}
	}
}
