using System;
using System.Reflection;

namespace Given.Common
{
    public interface ITestRunnerConfiguration
    {
        Func<MethodInfo, bool> ThenIdentificationMethod { get; }
    }
}