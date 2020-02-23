using System;

namespace Retro.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        private const string DefaultMessage = "{0} with Id = {1} was not found.";

        protected NotFoundException(string name, string id) : base(string.Format(DefaultMessage, name, id))
        {
        }
    }
}