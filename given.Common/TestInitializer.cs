using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Given.Common
{
    public class TestInitializer
    {
        const string print = "{0} {1}";
        const string and = "     And";
        readonly Type _typeToProcess;
        readonly object _testClass;
        readonly bool _logToConsole;
        string then = "Then";
        string given = "Given";
        string when = "When";
        List<FieldInfo> _fields;

        public TestInitializer(object testClass, bool logToConsole)
        {
            _typeToProcess = testClass.GetType();
            _testClass = testClass;
            _logToConsole = logToConsole;
            DetermineFields();
        }

        void DetermineFields()
        {
            var currentType = _typeToProcess;
            _fields = currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).ToList();

            while ((currentType = currentType.BaseType) != null)
                _fields.InsertRange(0, currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
        }

        public void ProcessWhen()
        {
            //execute all cause items found.
            _fields.Where(fieldInfo => fieldInfo.FieldType == typeof (when))
                   .Select(fieldInfo => new {Delegate = (when) fieldInfo.GetValue(_testClass), Field = fieldInfo}).ToList()
                   .ForEach(x =>
                                {
                                    x.Delegate.Invoke();
                                    if (_logToConsole) Console.WriteLine(print, when, x.Field.Name.Replace("_", " "));
                                    when = and;
                                });
        }

        public void ProcessGiven()
        {
            //execute all given items found.
            _fields.Where(fieldInfo => fieldInfo.FieldType == typeof (given))
                   .Select(fieldInfo => new { Delegate = (given)fieldInfo.GetValue(_testClass), Field = fieldInfo }).ToList()
                   .ForEach(x =>
                                {
                                    x.Delegate.Invoke();
                                    if (_logToConsole) Console.WriteLine(print, given, x.Field.Name.Replace("_", " "));
                                    given = and;
                                });
        }

        public void ProcessThen()
        {
            _typeToProcess
                .GetMethods()
                .Where(methodInfo => methodInfo.GetCustomAttributes(typeof (IThenAttribute), false).Any()).ToList()
                .ForEach(x =>
                             {
                                 Console.WriteLine(print, then, x.Name.Replace("_", " "));
                                 then = and;
                             });
        }
    }
}