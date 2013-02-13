using System;
using System.Linq;
using System.Reflection;

namespace Given.Common
{
    public class DefaultTestRunnerConfiguration : ITestRunnerConfiguration
    {
        public Func<MethodInfo, bool> ThenIdentificationMethod
        {
            get { return methodInfo => methodInfo.GetCustomAttributes(typeof (IThenAttribute), false).Any(); }
        }
    }
}