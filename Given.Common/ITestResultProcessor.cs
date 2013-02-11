using System;
using System.Collections.Generic;

namespace Given.Common
{
    public interface ITestResultProcessor
    {
        void Process(IEnumerable<TestResult> testResults, Guid testRunId);
    }
}