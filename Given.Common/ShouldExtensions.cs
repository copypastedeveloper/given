//Copyright (c) 2008 Machine Project
//Portions Copyright (c) 2008 Jacob Lewallen, Aaron Jensen

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Given.Common
{
    [Serializable]
    public class SpecificationException : Exception
    {
        public SpecificationException()
        {
        }

        public SpecificationException(string message)
            : base(message)
        {
        }

        public SpecificationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SpecificationException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }

    public static class ShouldExtensionMethods
    {
        public static void ShouldBeFalse(this bool condition)
        {
            if (condition)
                throw new SpecificationException("Should be [false] but is [true]");
        }


        public static void ShouldBeTrue(this bool condition)
        {
            if (!condition)
                throw new SpecificationException("Should be [true] but is [false]");
        }

        public static T ShouldEqual<T>(this T actual, T expected)
        {
            if (!actual.Equals(expected))
            {
                throw new SpecificationException(string.Format("  Expected: {1}{0}  But was:  {2}", Environment.NewLine,
                                                               expected.ToUsefulString(), actual.ToUsefulString()));
            }

            return actual;
        }

        public static object ShouldNotEqual<T>(this T actual, T expected)
        {
            if (actual.Equals(expected))
            {
                throw new SpecificationException(string.Format("Should not equal {0} but does: {1}",
                                                               expected.ToUsefulString(),
                                                               actual.ToUsefulString()));
            }

            return actual;
        }


        public static void ShouldBeNull(this object anObject)
        {
            if (anObject != null)
            {
                throw new SpecificationException(string.Format("Should be [null] but is {0}", anObject.ToUsefulString()));
            }
        }


        public static void ShouldNotBeNull(this object anObject)
        {
            if (anObject == null)
            {
                throw new SpecificationException(string.Format("Should be [not null] but is [null]"));
            }
        }

        public static object ShouldBeTheSameAs(this object actual, object expected)
        {
            if (!ReferenceEquals(actual, expected))
            {
                throw new SpecificationException(string.Format("Should be the same as {0} but is {1}", expected, actual));
            }

            return expected;
        }

        public static object ShouldNotBeTheSameAs(this object actual, object expected)
        {
            if (ReferenceEquals(actual, expected))
            {
                throw new SpecificationException(string.Format("Should not be the same as {0} but is {1}", expected,
                                                               actual));
            }

            return expected;
        }

        public static void ShouldBeOfType(this object actual, Type expected)
        {
            if (actual == null)
            {
                throw new SpecificationException(string.Format("Should be of type {0} but is [null]", expected));
            }

            if (!expected.IsAssignableFrom(actual.GetType()))
            {
                throw new SpecificationException(string.Format("Should be of type {0} but is of type {1}",
                                                               expected,
                                                               actual.GetType()));
            }
        }

        public static void ShouldBeOfType<T>(this object actual)
        {
            actual.ShouldBeOfType(typeof (T));
        }

        public static void ShouldBe(this object actual, Type expected)
        {
            actual.ShouldBeOfType(expected);
        }

        public static void ShouldNotBeOfType(this object actual, Type expected)
        {
            if (actual.GetType() == expected)
            {
                throw new SpecificationException(string.Format("Should not be of type {0} but is of type {1}", expected,
                                                               actual.GetType()));
            }
        }

        public static void ShouldMatch<T>(this T actual, Expression<Func<T, bool>> condition)
        {
            bool matches = condition.Compile().Invoke(actual);

            if (matches) return;
            throw new SpecificationException(string.Format("Should match expression [{0}], but does not.", condition));
        }

        public static void ShouldEachConformTo<T>(this IEnumerable<T> list, Expression<Func<T, bool>> condition)
        {
            var source = new List<T>(list);
            Func<T, bool> func = condition.Compile();

            IEnumerable<T> failingItems = source.Where(x => func(x) == false);

            if (failingItems.Any())
            {
                throw new SpecificationException(string.Format(
                    @"Should contain only elements conforming to: {0}
the following items did not meet the condition: {1}",
                    condition,
                    failingItems.EachToUsefulString()));
            }
        }

        public static void ShouldContain(this IEnumerable list, params object[] items)
        {
            IEnumerable<object> actualList = list.Cast<object>();
            IEnumerable<object> expectedList = items.Cast<object>();

            actualList.ShouldContain(expectedList);
        }

        public static void ShouldContain<T>(this IEnumerable<T> list, params T[] items)
        {
            list.ShouldContain((IEnumerable<T>) items);
        }

        public static void ShouldContain<T>(this IEnumerable<T> list, IEnumerable<T> items)
        {
            List<T> noContain = items.Where(item => !list.Contains(item)).ToList();

            if (noContain.Any())
            {
                throw new SpecificationException(string.Format(
                    @"Should contain: {0} 
                      entire list: {1}
                      does not contain: {2}",
                    items.EachToUsefulString(),
                    list.EachToUsefulString(),
                    noContain.EachToUsefulString()));
            }
        }

        public static void ShouldContain<T>(this IEnumerable<T> list, Expression<Func<T, bool>> condition)
        {
            Func<T, bool> func = condition.Compile();

            if (!list.Any(func))
            {
                throw new SpecificationException(string.Format(
                    @"Should contain elements conforming to: {0}
                      entire list: {1}",
                    condition,
                    list.EachToUsefulString()));
            }
        }

        public static void ShouldNotContain(this IEnumerable list, params object[] items)
        {
            IEnumerable<object> actualList = list.Cast<object>();
            IEnumerable<object> expectedList = items.Cast<object>();

            actualList.ShouldNotContain(expectedList);
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> list, params T[] items)
        {
            list.ShouldNotContain((IEnumerable<T>) items);
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> list, IEnumerable<T> items)
        {
            List<T> contains = items.Where(list.Contains).ToList();

            if (contains.Any())
            {
                throw new SpecificationException(string.Format(
                    @"Should not contain: {0} 
                        entire list: {1}
                        does contain: {2}",
                    items.EachToUsefulString(),
                    list.EachToUsefulString(),
                    contains.EachToUsefulString()));
            }
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> list, Expression<Func<T, bool>> condition)
        {
            Func<T, bool> func = condition.Compile();

            IEnumerable<T> contains = list.Where(func);

            if (contains.Any())
            {
                throw new SpecificationException(string.Format(
                    @"No elements should conform to: {0}
                        entire list: {1}
                        does contain: {2}",
                    condition,
                    list.EachToUsefulString(),
                    contains.EachToUsefulString()));
            }
        }

        static SpecificationException NewException(string message, params object[] parameters)
        {
            if (parameters.Any())
            {
                return
                    new SpecificationException(string.Format(message,
                                                             parameters.Select(x => x.ToUsefulString())
                                                                       .Cast<object>()
                                                                       .ToArray()));
            }
            return new SpecificationException(message);
        }

        public static IComparable ShouldBeGreaterThan(this IComparable arg1, IComparable arg2)
        {
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg1 == null) throw NewException("Should be greater than {0} but is [null]", arg2);

            if (arg1.CompareTo(arg2.TryToChangeType(arg1.GetType())) <= 0)
                throw NewException("Should be greater than {0} but is {1}", arg2, arg1);

            return arg1;
        }

        public static IComparable ShouldBeGreaterThanOrEqualTo(this IComparable arg1, IComparable arg2)
        {
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg1 == null) throw NewException("Should be greater than or equal to {0} but is [null]", arg2);

            if (arg1.CompareTo(arg2.TryToChangeType(arg1.GetType())) < 0)
                throw NewException("Should be greater than or equal to {0} but is {1}", arg2, arg1);

            return arg1;
        }

        static object TryToChangeType(this object original, Type type)
        {
            try
            {
                return Convert.ChangeType(original, type);
            }
            catch
            {
                return original;
            }
        }

        public static IComparable ShouldBeLessThan(this IComparable arg1, IComparable arg2)
        {
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg1 == null) throw NewException("Should be less than {0} but is [null]", arg2);

            if (arg1.CompareTo(arg2.TryToChangeType(arg1.GetType())) >= 0)
                throw NewException("Should be less than {0} but is {1}", arg2, arg1);

            return arg1;
        }

        public static IComparable ShouldBeLessThanOrEqualTo(this IComparable arg1, IComparable arg2)
        {
            if (arg2 == null) throw new ArgumentNullException("arg2");
            if (arg1 == null) throw NewException("Should be less than or equal to {0} but is [null]", arg2);

            if (arg1.CompareTo(arg2.TryToChangeType(arg1.GetType())) > 0)
                throw NewException("Should be less than or equal to {0} but is {1}", arg2, arg1);

            return arg1;
        }

        public static void ShouldBeCloseTo(this float actual, float expected)
        {
            ShouldBeCloseTo(actual, expected, 0.0000001f);
        }

        public static void ShouldBeCloseTo(this float actual, float expected, float tolerance)
        {
            if (Math.Abs(actual - expected) > tolerance)
            {
                throw new SpecificationException(string.Format("Should be within {0} of {1} but is {2}",
                                                               tolerance.ToUsefulString(),
                                                               expected.ToUsefulString(),
                                                               actual.ToUsefulString()));
            }
        }

        public static void ShouldBeCloseTo(this double actual, double expected)
        {
            ShouldBeCloseTo(actual, expected, 0.0000001f);
        }

        public static void ShouldBeCloseTo(this double actual, double expected, double tolerance)
        {
            if (Math.Abs(actual - expected) > tolerance)
            {
                throw new SpecificationException(string.Format("Should be within {0} of {1} but is {2}",
                                                               tolerance.ToUsefulString(),
                                                               expected.ToUsefulString(),
                                                               actual.ToUsefulString()));
            }
        }

        public static void ShouldBeCloseTo(this TimeSpan actual, TimeSpan expected, TimeSpan tolerance)
        {
            if (Math.Abs(actual.Ticks - expected.Ticks) > tolerance.Ticks)
            {
                throw new SpecificationException(string.Format("Should be within {0} of {1} but is {2}",
                                                               tolerance.ToUsefulString(),
                                                               expected.ToUsefulString(),
                                                               actual.ToUsefulString()));
            }
        }

        public static void ShouldBeCloseTo(this DateTime actual, DateTime expected, TimeSpan tolerance)
        {
            TimeSpan difference = expected - actual;
            if (Math.Abs(difference.Ticks) > tolerance.Ticks)
            {
                throw new SpecificationException(string.Format("Should be within {0} of {1} but is {2}",
                                                               tolerance.ToUsefulString(),
                                                               expected.ToUsefulString(),
                                                               actual.ToUsefulString()));
            }
        }

        public static void ShouldBeEmpty(this IEnumerable collection)
        {
            if (collection.Cast<object>().Any())
            {
                throw NewException("Should be empty but contains:\n" + collection.Cast<object>().EachToUsefulString());
            }
        }

        public static void ShouldBeEmpty(this string aString)
        {
            if (aString == null)
            {
                throw new SpecificationException("Should be empty but is [null]");
            }

            if (!string.IsNullOrEmpty(aString))
            {
                throw NewException("Should be empty but is {0}", aString);
            }
        }

        public static void ShouldNotBeEmpty(this IEnumerable collection)
        {
            if (!collection.Cast<object>().Any())
            {
                throw NewException("Should not be empty but is");
            }
        }

        public static void ShouldNotBeEmpty(this string aString)
        {
            if (string.IsNullOrEmpty(aString))
            {
                throw NewException("Should not be empty but is");
            }
        }

        public static void ShouldMatch(this string actual, string pattern)
        {
            if (pattern == null) throw new ArgumentNullException("pattern");
            if (actual == null) throw NewException("Should match regex {0} but is [null]", pattern);

            ShouldMatch(actual, new Regex(pattern));
        }

        public static void ShouldMatch(this string actual, Regex pattern)
        {
            if (pattern == null) throw new ArgumentNullException("pattern");
            if (actual == null) throw NewException("Should match regex {0} but is [null]", pattern);

            if (!pattern.IsMatch(actual))
            {
                throw NewException("Should match {0} but is {1}", pattern, actual);
            }
        }

        public static void ShouldContain(this string actual, string expected)
        {
            if (expected == null) throw new ArgumentNullException("expected");
            if (actual == null) throw NewException("Should contain {0} but is [null]", expected);

            if (!actual.Contains(expected))
            {
                throw NewException("Should contain {0} but is {1}", expected, actual);
            }
        }

        public static void ShouldNotContain(this string actual, string notExpected)
        {
            if (notExpected == null) throw new ArgumentNullException("notExpected");
            if (actual == null) return;

            if (actual.Contains(notExpected))
            {
                throw NewException("Should not contain {0} but is {1}", notExpected, actual);
            }
        }

        public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
        {
            if (expected == null) throw new ArgumentNullException("expected");
            if (actual == null) throw NewException("Should be equal ignoring case to {0} but is [null]", expected);

            if (!actual.Equals(expected, StringComparison.InvariantCultureIgnoreCase))
            {
                throw NewException("Should be equal ignoring case to {0} but is {1}", expected, actual);
            }

            return actual;
        }

        public static void ShouldStartWith(this string actual, string expected)
        {
            if (expected == null) throw new ArgumentNullException("expected");
            if (actual == null) throw NewException("Should start with {0} but is [null]", expected);

            if (!actual.StartsWith(expected))
            {
                throw NewException("Should start with {0} but is {1}", expected, actual);
            }
        }

        public static void ShouldEndWith(this string actual, string expected)
        {
            if (expected == null) throw new ArgumentNullException("expected");
            if (actual == null) throw NewException("Should end with {0} but is [null]", expected);

            if (!actual.EndsWith(expected))
            {
                throw NewException("Should end with {0} but is {1}", expected, actual);
            }
        }

        public static void ShouldBeSurroundedWith(this string actual, string expectedStartDelimiter,
                                                  string expectedEndDelimiter)
        {
            actual.ShouldStartWith(expectedStartDelimiter);
            actual.ShouldEndWith(expectedEndDelimiter);
        }

        public static void ShouldBeSurroundedWith(this string actual, string expectedDelimiter)
        {
            actual.ShouldStartWith(expectedDelimiter);
            actual.ShouldEndWith(expectedDelimiter);
        }

        public static void ShouldContainErrorMessage(this Exception exception, string expected)
        {
            exception.Message.ShouldContain(expected);
        }

        public static void ShouldContainOnly<T>(this IEnumerable<T> list, params T[] items)
        {
            list.ShouldContainOnly((IEnumerable<T>) items);
        }

        public static void ShouldContainOnly<T>(this IEnumerable<T> list, IEnumerable<T> items)
        {
            var source = new List<T>(list);
            var noContain = new List<T>();

            foreach (T item in items)
            {
                if (!source.Contains(item))
                {
                    noContain.Add(item);
                }
                else
                {
                    source.Remove(item);
                }
            }

            if (noContain.Any() || source.Any())
            {
                string message = string.Format(@"Should contain only: {0} entire list: {1}",
                                               items.EachToUsefulString(),
                                               list.EachToUsefulString());
                if (noContain.Any())
                {
                    message += "\ndoes not contain: " + noContain.EachToUsefulString();
                }
                if (source.Any())
                {
                    message += "\ndoes contain but shouldn't: " + source.EachToUsefulString();
                }

                throw new SpecificationException(message);
            }
        }

        public static void ShouldBeInTheSameOrderAs<T>(this IEnumerable<T> firstEnumerable, IEnumerable<T> secondEnumerable)
        {
            if (!firstEnumerable.SequenceEqual(secondEnumerable))
            {
                throw new SpecificationException("Expected enumerated items to be in the same order, but they were not.");
            }
        }

        public static Exception ShouldBeThrownBy(this Type exceptionType, Action method)
        {
            Exception exception = Catch.Exception(method.Invoke);

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(exceptionType);
            return exception;
        }

        public static string EachToUsefulString<T>(this IEnumerable<T> enumerable)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.Append(String.Join(",\n", enumerable.Select(x => x.ToUsefulString()).Take(10).ToArray()));
            if (enumerable.Count() > 10)
            {
                if (enumerable.Count() > 11)
                {
                    sb.AppendLine(String.Format(",\n  ...({0} more elements)", enumerable.Count() - 10));
                }
                else
                {
                    sb.AppendLine(",\n" + enumerable.Last().ToUsefulString());
                }
            }
            else
            {
                sb.AppendLine();
            }
            sb.AppendLine("}");

            return sb.ToString();
        }

        internal static string ToUsefulString(this object obj)
        {
            string str;
            if (obj == null)
            {
                return "[null]";
            }
            if (obj.GetType() == typeof (string))
            {
                str = (string) obj;

                return "\"" + str.Replace("\n", "\\n") + "\"";
            }
            if (obj.GetType().IsValueType)
            {
                return "[" + obj + "]";
            }

            if (obj is IEnumerable)
            {
                IEnumerable<object> enumerable = ((IEnumerable) obj).Cast<object>();

                return obj.GetType() + ":\n" + enumerable.EachToUsefulString();
            }

            str = obj.ToString();

            if (str == null || str.Trim() == "")
            {
                return String.Format("{0}:[]", obj.GetType());
            }

            str = str.Trim();

            if (str.Contains("\n"))
            {
                return string.Format(@"{1}:[{0}]", str, obj.GetType());
            }

            if (obj.GetType().ToString() == str)
            {
                return obj.GetType().ToString();
            }

            return string.Format("{0}:[{1}]", obj.GetType(), str);
        }
    }
}