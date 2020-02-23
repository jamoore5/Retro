using System;

namespace Retro.Exceptions
{
    public class InvalidIncludeException : Exception
    {
        private const string DefaultMessage = "Invalid include parameter.";

        public InvalidIncludeException() : base(DefaultMessage)
        {
        }
    }
}