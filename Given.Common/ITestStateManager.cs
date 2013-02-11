using System;
using System.Reflection;

namespace Given.Common
{
    public interface ITestStateManager
    {
        void AddGiven(string text, Delegate method);
        void AddWhen(string text, Delegate method);
        void AddThen(string text, MethodInfo method);
        void SetThenState(string then, TestState state);
        void WriteSpecification(Action<string,object[]> consoleAction = null);
    }
}