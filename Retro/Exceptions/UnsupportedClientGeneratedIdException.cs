using System;

namespace Retro.Exceptions
{
    public class UnsupportedClientGeneratedIdException : Exception
    {
        private const string DefaultMessage = "Id cannot be set by the client.";

        public UnsupportedClientGeneratedIdException() : base(DefaultMessage)
        {
        }
    }
}