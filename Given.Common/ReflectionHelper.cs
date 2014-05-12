using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Given.Common
{
    internal class ReflectionHelper
    {
        internal static List<Type> ConcreteTypes()
        {
            List<Type> concreteTypes = new List<Type>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    Type[] types = assembly.GetTypes(); // This line can throw ReflectionTypeLoadException.
                    concreteTypes.AddRange(types.Where(x => !x.IsAbstract && !x.IsGenericTypeDefinition && !x.IsInterface && x.IsPublic));
                }
                catch (ReflectionTypeLoadException e)
                {
                    Console.WriteLine(FormatLoaderExceptionMessage(assembly, e));
                }
            }

            return concreteTypes;
        }

        static string FormatLoaderExceptionMessage(Assembly assembly, ReflectionTypeLoadException e)
        {
            //http://stackoverflow.com/questions/7889228/how-to-prevent-reflectiontypeloadexception-when-calling-assembly-gettypes
            StringBuilder message = new StringBuilder();
            message.AppendLine(string.Format("{0} assembly LoaderException messages:", assembly.FullName));
            foreach (Exception loaderException in e.LoaderExceptions)
            {
                if (loaderException == null) continue;
                message.AppendLine(loaderException.Message);
            }
            return message.ToString();
        }
    }
}
