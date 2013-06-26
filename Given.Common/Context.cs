// ReSharper disable PossibleNullReferenceException
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Given.Common
{
    public static class Context
    {
        internal static readonly ContextDictionary Contexts = new ContextDictionary();

        internal static readonly Dictionary<string, Delegate> CleanUps = new Dictionary<string, Delegate>();
        internal static readonly Dictionary<string, object> TestRunContext = new Dictionary<string, object>();

        static Context()
        {
            var concreteTypes = AppDomain.CurrentDomain.GetAssemblies()
                                         .SelectMany(assembly => assembly.GetTypes()).ToList()
                                         .Where(x => x.IsAbstract == false &&
                                                     x.IsGenericTypeDefinition == false &&
                                                     x.IsInterface == false).ToList();

            //initialize context providers
            concreteTypes.Where(x => typeof(IContextProvider).IsAssignableFrom(x)).ToList()
                         .ForEach(x => ((IContextProvider)Activator.CreateInstance(x)).SetupContext());

        }

        public static SetupHelper Register(string context)
        {
            return new SetupHelper(context);
        }

        public static GivenResult Given(string context)
        {
            var currentTest = new StackFrame(1).GetMethod().DeclaringType.Name;
            var key = currentTest + context;

            if (TestRunContext.ContainsKey(key)) return new GivenResult {Executed = false};

            TestRunManager.AddTransientGiven(currentTest, context, Contexts[context]);

            var given = Contexts[context];
            var result = given.DynamicInvoke();

            TestRunContext.Add(key, result);

            return new GivenResult {Executed = true};
        }

        public static T1 Given<T1>(string context)
        {
            var currentTest = new StackFrame(1).GetMethod().DeclaringType.Name;
            var key = currentTest + context;

            if (!TestRunContext.ContainsKey(key))
            {
                var given = ((given<T1>)Contexts[context]);
                TestRunManager.AddTransientGiven(currentTest, context, given);
                TestRunContext.Add(key, ((given<T1>)Contexts[context]).Invoke());
            }

            return (T1)TestRunContext[key];
        }

        public static Tuple<T1, T2> Given<T1, T2>(string context)
        {
            var currentTest = new StackFrame(1).GetMethod().DeclaringType.Name;
            var key = currentTest + context;

            if (!TestRunContext.ContainsKey(key))
            {
                var given = ((given<T1, T2>)Contexts[context]);
                TestRunManager.AddTransientGiven(currentTest, context, given);
                TestRunContext.Add(key, ((given<T1, T2>)Contexts[context]).Invoke());
            }

            return (Tuple<T1, T2>)TestRunContext[key];
        }

        public static Tuple<T1, T2, T3> Given<T1, T2, T3>(string context)
        {
            var currentTest = new StackFrame(1).GetMethod().DeclaringType.Name;
            var key = currentTest + context;

            if (!TestRunContext.ContainsKey(key))
            {
                var given = ((given<T1, T2, T3>)Contexts[context]);
                TestRunManager.AddTransientGiven(currentTest, context, given);
                TestRunContext.Add(key, ((given<T1, T2, T3>)Contexts[context]).Invoke());
            }

            return (Tuple<T1, T2, T3>)TestRunContext[key];
        }

        public static Tuple<T1, T2, T3, T4> Given<T1, T2, T3, T4>(string context)
        {
            var currentTest = new StackFrame(1).GetMethod().DeclaringType.Name;

            var key = currentTest + context;

            if (!TestRunContext.ContainsKey(key))
            {
                var given = ((given<T1, T2, T3, T4>)Contexts[context]);
                TestRunManager.AddTransientGiven(currentTest, context, given);
                TestRunContext.Add(key, ((given<T1, T2, T3, T4>)Contexts[context]).Invoke());
            }

            return (Tuple<T1, T2, T3, T4>)TestRunContext[key];
        }

        public static Tuple<T1, T2, T3, T4, T5> Given<T1, T2, T3, T4, T5>(string context)
        {
            var currentTest = new StackFrame(1).GetMethod().DeclaringType.Name;
            var key = currentTest + context;

            if (!TestRunContext.ContainsKey(key))
            {
                var given = ((given<T1, T2, T3, T4, T5>)Contexts[context]);
                TestRunManager.AddTransientGiven(currentTest, context, given);
                TestRunContext.Add(key, ((given<T1, T2, T3, T4, T5>)Contexts[context]).Invoke());
            }

            return (Tuple<T1, T2, T3, T4, T5>)TestRunContext[key];
        }
    }
}