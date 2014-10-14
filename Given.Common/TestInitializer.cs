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
        readonly TestStateManager _testStateManager;
        List<FieldInfo> _fields;

        public TestInitializer(object testClass, TestStateManager testStateManager)
        {
            _typeToProcess = testClass.GetType();
            _testClass = testClass;
            _testStateManager = testStateManager;
            DetermineFields();
        }

        void DetermineFields()
        {
            var currentType = _typeToProcess;
            _fields = currentType.GetFields(TestRunManager.FieldsToRetrieve).ToList();

            while ((currentType = currentType.BaseType) != null)
                _fields.InsertRange(0, currentType.GetFields(TestRunManager.FieldsToRetrieve));
        }

        public void ProcessDelegates()
        {
            ProcessBefore();
            ProcessGiven();
            ProcessWhen();
            ProcessThen();
            ProcessAfter();
        }

        void ProcessBefore()
        {
            //execute all before items found.
            _fields.Where(fieldInfo => fieldInfo.FieldType == typeof(before))
                   .Select(fieldInfo => new { Delegate = (before)fieldInfo.GetValue(_testClass), Field = fieldInfo }).ToList()
                   .ForEach(x =>
                   {
                       x.Delegate.Invoke();
                   });
        }

        void ProcessGiven()
        {
            //execute all given items found.
            _fields.Where(fieldInfo => fieldInfo.FieldType == typeof(given))
                   .Select(fieldInfo => new { Delegate = (given)fieldInfo.GetValue(_testClass), Field = fieldInfo }).ToList()
                   .ForEach(x =>
                   {
                       x.Delegate.Invoke();
                       _testStateManager.AddGiven(x.Field.Name, x.Delegate);
                   });
        }

        void ProcessWhen()
        {
            //execute all when items found.
            _fields.Where(fieldInfo => fieldInfo.FieldType == typeof(when))
                   .Select(fieldInfo => new { Delegate = (when)fieldInfo.GetValue(_testClass), Field = fieldInfo }).ToList()
                   .ForEach(x =>
                   {

                       x.Delegate.Invoke();
                       _testStateManager.AddWhen(x.Field.Name, x.Delegate);
                   });
        }

        void ProcessThen()
        {
            _typeToProcess
                .GetMethods()
                .Where(TestRunManager.TestRunConfiguration.ThenIdentificationMethod).ToList()
                .ForEach(x => _testStateManager.AddThen(x.Name, x));
        }

        void ProcessAfter()
        {
            _fields
                .Where(fieldInfo => fieldInfo.FieldType == typeof(after)).ToList()
                .ForEach(x => _testStateManager.AddTearDown((after)x.GetValue(_testClass)));
        }
    }
}