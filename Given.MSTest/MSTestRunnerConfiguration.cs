using System;
using System.Linq;
using System.Reflection;
using Given.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Given.MSTest
{
    public class MSTestRunnerConfiguration : ITestRunnerConfiguration
    {
        public Func<MethodInfo, bool> ThenIdentificationMethod
        {
            get { return info => info.GetCustomAttributes(typeof (TestMethodAttribute),false).Any(); }
        }
    }
}