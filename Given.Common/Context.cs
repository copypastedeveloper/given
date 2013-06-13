using System;
using System.Collections.Generic;

namespace Given.Common
{
    public static class Context
    {
        static readonly Dictionary<string, object> Contexts = new Dictionary<string, object>();

        static readonly Dictionary<string, object> TestRunContext = new Dictionary<string, object>();

        public static void Register(string context, given given)
        {
            Contexts.Add(context, given);
        }

        public static void Register<T1>(string context, given<T1> given)
        {
            Contexts.Add(context, given);
        }

        public static void Register<T1, T2>(string context, given<T1, T2> given)
        {
            Contexts.Add(context, given);
        }

        public static void Register<T1, T2, T3>(string context, given<T1, T2, T3> given)
        {
            Contexts.Add(context, given);
        }


        public static void Register<T1, T2, T3, T4>(string context, given<T1, T2, T3, T4> given)
        {
            Contexts.Add(context, given);
        }


        public static void Register<T1, T2, T3, T4, T5>(string context, given<T1, T2, T3, T4, T5> given)
        {
            Contexts.Add(context, given);
        }

        public static void Given(string context)
        {
            var currentTest = TestRunManager.CurrentTestRun.CurrentTest;
            var key = currentTest.GetHashCode() + context;

            if (TestRunContext.ContainsKey(key)) return;

            var given = ((given)Contexts[context]);
            currentTest.AddGiven(context, given);
                
            given.Invoke();
                
            TestRunContext.Add(key, null);
        }

        public static Lazy<T1> Given<T1>(string context)
        {

            return new Lazy<T1>(() =>
                                    {
                                        var currentTest = TestRunManager.CurrentTestRun.CurrentTest;
                                        var key = currentTest.GetHashCode() + context;

                                        if (!TestRunContext.ContainsKey(key))
                                        {
                                            var given = ((given<T1>)Contexts[context]);
                                            currentTest.AddGiven(context, given);
                                            TestRunContext.Add(key, ((given<T1>)Contexts[context]).Invoke());
                                        }

                                        return (T1)TestRunContext[key];
                                    });
        }

        public static Lazy<Tuple<T1, T2>> Given<T1, T2>(string context)
        {
            return new Lazy<Tuple<T1, T2>>(() =>
                                               {
                                                   var currentTest = TestRunManager.CurrentTestRun.CurrentTest;
                                                   var key = currentTest.GetHashCode() + context;

                                                   if (!TestRunContext.ContainsKey(key))
                                                   {
                                                       var given = ((given<T1, T2>)Contexts[context]);
                                                       currentTest.AddGiven(context, given);
                                                       TestRunContext.Add(key, ((given<T1>)Contexts[context]).Invoke());
                                                   }

                                                   return (Tuple<T1, T2>)TestRunContext[key];
                                               });
        }

        public static Lazy<Tuple<T1, T2, T3>> Given<T1, T2, T3>(string context)
        {
            return new Lazy<Tuple<T1, T2, T3>>(() =>
                                                   {
                                                       var currentTest = TestRunManager.CurrentTestRun.CurrentTest;
                                                       var key = currentTest.GetHashCode() + context;

                                                       if (!TestRunContext.ContainsKey(key))
                                                       {
                                                           var given = ((given<T1, T2, T3>)Contexts[context]);
                                                           currentTest.AddGiven(context, given);
                                                           TestRunContext.Add(key, ((given<T1>)Contexts[context]).Invoke());
                                                       }

                                                       return (Tuple<T1, T2, T3>)TestRunContext[key];
                                                   });
        }

        public static Lazy<Tuple<T1, T2, T3, T4>> Given<T1, T2, T3, T4>(string context)
        {
            return new Lazy<Tuple<T1, T2, T3, T4>>(() =>
                                                   {
                                                       var currentTest = TestRunManager.CurrentTestRun.CurrentTest;
                                                       var key = currentTest.GetHashCode() + context;

                                                       if (!TestRunContext.ContainsKey(key))
                                                       {
                                                           var given = ((given<T1, T2, T3, T4>)Contexts[context]);
                                                           currentTest.AddGiven(context, given);
                                                           TestRunContext.Add(key, ((given<T1>)Contexts[context]).Invoke());
                                                       }

                                                       return (Tuple<T1, T2, T3, T4>)TestRunContext[key];
                                                   });
        }

        public static Lazy<Tuple<T1, T2, T3, T4, T5>> Given<T1, T2, T3, T4, T5>(string context)
        {
            return new Lazy<Tuple<T1, T2, T3, T4, T5>>(() =>
                                                   {
                                                       var currentTest = TestRunManager.CurrentTestRun.CurrentTest;
                                                       var key = currentTest.GetHashCode() + context;

                                                       if (!TestRunContext.ContainsKey(key))
                                                       {
                                                           var given = ((given<T1, T2, T3, T4, T5>)Contexts[context]);
                                                           currentTest.AddGiven(context, given);
                                                           TestRunContext.Add(key, ((given<T1>)Contexts[context]).Invoke());
                                                       }

                                                       return (Tuple<T1, T2, T3, T4, T5>)TestRunContext[key];
                                                   });
        }
    }
}