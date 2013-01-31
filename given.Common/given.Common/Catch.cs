using System;

namespace Given.Common
{
    public static class Catch
    {
        public static Exception Exception(MethodThatThrows method)
        {
            try
            {
                method();
            }
            catch (Exception e)
            {
                return e;
            }

            return
                new DidNotThrowAnExceptionException(string.Format("Expected call to throw an exception, but it did not."));
        }
    }
}