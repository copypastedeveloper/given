using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Given.Common
{
    public class TestInitializer
    {
        readonly Type _typeToProcess;
        readonly object _testClass;
        List<FieldInfo> _fields;

        public TestInitializer(object testClass)
        {
            _typeToProcess = testClass.GetType();
            _testClass = testClass;
            DetermineFields();
        }

        void DetermineFields()
        {
            var currentType = _typeToProcess;
            _fields = currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).ToList();

            while ((currentType = currentType.BaseType) != null)
                _fields.InsertRange(0, currentType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
        }

        public void ProcessGiven(ITestStateManager testStateManager)
        {
            //execute all given items found.
            _fields.Where(fieldInfo => fieldInfo.FieldType == typeof(given))
                   .Select(fieldInfo => new { Delegate = (given)fieldInfo.GetValue(_testClass), Field = fieldInfo }).ToList()
                   .ForEach(x =>
                   {
                       x.Delegate.Invoke();
                       testStateManager.AddGiven(x.Field.Name, x.Delegate);
                   });
        }

        public void ProcessWhen(ITestStateManager testStateManager)
        {
            //execute all cause items found.
            _fields.Where(fieldInfo => fieldInfo.FieldType == typeof (when))
                   .Select(fieldInfo => new {Delegate = (when) fieldInfo.GetValue(_testClass), Field = fieldInfo}).ToList()
                   .ForEach(x =>
                                {
                                    
                                    x.Delegate.Invoke();
                                    testStateManager.AddWhen(x.Field.Name,x.Delegate);
                                });
        }

        public void ProcessThen(ITestStateManager testStateManager)
        {
            _typeToProcess
                .GetMethods()
                .Where(methodInfo => methodInfo.GetCustomAttributes(typeof (IThenAttribute), false).Any()).ToList()
                .ForEach(x => testStateManager.AddThen(x.Name, x));
        }
    }
}