using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Given.Common
{
    public class TestStateManager : ITestStateManager
    {

        public Type TestType { get; set; }
        public PairList<Delegate, string> Givens { get; private set; }
        public PairList<Delegate, string> Whens { get; private set; }
        public PairList<MethodInfo, StatedThen> Thens { get; private set; }
        
        public TestStateManager(object specification)
        {
            TestType = specification.GetType();
            Givens = new PairList<Delegate, string>();
            Whens = new PairList<Delegate, string>();
            Thens = new PairList<MethodInfo, StatedThen>();
            TestRunManager.CurrentTestRun.AddTest(this, specification.GetType());
        }

        public void AddGiven(string text, Delegate method)
        {
            Givens.Add(method, text);
        }

        public void AddWhen(string text, Delegate method)
        {
            Whens.Add(method, text);
        }

        public void AddThen(string text, MethodInfo method)
        {
            Thens.Add(method, new StatedThen { Name = text });
        }

        public void SetThenState(string methodName, TestState state, string message)
        {
            var then = Thens.First(x => x.Key.Name == methodName).Value;
            then.State = state;
            then.Message = message;
        }

        public void Cleanup()
        {
            try
            {
                foreach (var given in Givens.Select(x => new { Method = x.Key, Text = x.Value }))
                {
                    if (!Context.CleanUps.ContainsKey(given.Text)) continue;

                    var result = given.Method.DynamicInvoke();

                    //if it doesn't take arguments just execute it
                    if (!Context.CleanUps[given.Text].Method.GetParameters().Any())
                    {
                        Context.CleanUps[given.Text].DynamicInvoke();
                        continue;
                    }

                    //if it takes one argument pass in the result
                    if (Context.CleanUps[given.Text].Method.GetParameters().Count() == 1)
                    {
                        Context.CleanUps[given.Text].DynamicInvoke(result);
                        continue;
                    }

                    //if it takes multiple arguments parse the tuple and pass them in
                    var count = result.GetType().GetGenericArguments().Count();
                    if (count > 0)
                    {
                        var type = result.GetType();
                        var argList = new List<object>();

                        for (int i = 1; i <= count; i++)
                        {
                            argList.Add(type.GetProperty("Item" + i).GetValue(result, null));
                        }

                        Context.CleanUps[given.Text].DynamicInvoke(argList.ToArray());
                        continue;
                    }

                    Console.WriteLine("Found cleanup for {0} but didn't know how to call it", given.Text);
                }
            }
            catch
            {
            }
        }

        public void WriteSpecification(Action<string,object[]> consoleAction = null)
        {
            foreach (var given in TestRunManager.TransientGivens.Where(x => x.Key == TestType.Name).SelectMany(x => x.Value))
            {
                AddGiven(given.Key, given.Value);
            }

            consoleAction = consoleAction ?? Console.WriteLine;
            var currentPrefix = Text.Given;
            foreach (var pair in Givens)
            {
                consoleAction(Text.Print, new object[] {currentPrefix, pair.Value.Replace("_", " ")});
                currentPrefix = Text.And;
            }
            currentPrefix = Text.When;
            foreach (var pair in Whens)
            {
                consoleAction(Text.Print, new object[] {currentPrefix, pair.Value.Replace("_", " ")});
                currentPrefix = Text.And;
            }
            currentPrefix = Text.Then;
            foreach (var pair in Thens)
            {
                consoleAction(Text.Print, new object[] {currentPrefix, pair.Value.Name.Replace("_", " ")});
                if (!string.IsNullOrEmpty(pair.Value.Message))
                    consoleAction(pair.Value.Message, new object[0]);

                currentPrefix = Text.And;
            }
        }
    }
}