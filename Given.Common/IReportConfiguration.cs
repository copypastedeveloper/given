namespace Given.Common
{
    public interface IReportConfiguration
    {
        object TestRunId { get; }
        string AssemblyHeader { get; }
        string TestResultDirectory { get; }
    }
}