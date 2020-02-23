using System;
using System.Collections.Generic;

namespace Retro.Interfaces
{
    public interface ICardService
    {
        IEnumerable<ICard> GetCards(long boardId, string columnId);
        ICard GetCard(long boardId, string columnId, Guid id);
        void AddCard(long boardId, string columnId, ICard card);
        void DeleteCard(long boardId, string columnId, Guid id);
        void DeleteCards(long boardId);
    }
}