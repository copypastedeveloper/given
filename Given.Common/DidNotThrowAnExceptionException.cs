using System;
using System.Runtime.Serialization;

namespace Given.Common
{
    [Serializable]
    public class DidNotThrowAnExceptionException : Exception
    {
        public DidNotThrowAnExceptionException()
        {
        }

        public DidNotThrowAnExceptionException(string message)
            : base(message)
        {
        }

        public DidNotThrowAnExceptionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DidNotThrowAnExceptionException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}