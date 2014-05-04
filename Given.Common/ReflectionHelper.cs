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
            List<Type> concreteTypes;
            try
            {
                concreteTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes()).ToList()
                    .Where(x => x.IsAbstract == false &&
                                x.IsGenericTypeDefinition == false &&
                                x.IsInterface == false &&
                                x.IsPublic).ToList();
            }
            catch (ReflectionTypeLoadException e)
            {
                //http://stackoverflow.com/questions/7889228/how-to-prevent-reflectiontypeloadexception-when-calling-assembly-gettypes
                StringBuilder message = new StringBuilder();
                message.AppendLine("LoaderException messages:");
                foreach (Exception loaderException in e.LoaderExceptions)
                {
                    if (loaderException == null) continue;
                    message.AppendLine(loaderException.Message);
                }
                throw new Exception(message.ToString());
            }
            return concreteTypes;
        }
    }
}
