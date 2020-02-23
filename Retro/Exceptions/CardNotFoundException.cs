using System;

namespace Retro.Exceptions
{
    public class CardNotFoundException: NotFoundException
    {
        private const string Name = "Card";

        public CardNotFoundException(Guid id) : base(Name, id.ToString())
        {
        }
    }
}