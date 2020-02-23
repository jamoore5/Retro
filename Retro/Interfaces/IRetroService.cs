using System;
using System.Collections.Generic;

namespace Retro.Interfaces
{
    public interface IRetroService
    {
        IEnumerable<IBoard> GetBoards();

        IBoard GetBoard(long id, bool includeColumns = false, bool includeCards = false);

        void AddBoard(IBoard board);

        IEnumerable<IColumn> GetColumns(long boardId);

        IColumn GetColumn(long boardId, string id);

        void AddColumn(long boardId, IColumn column);

        void DeleteBoard(long id);

        void DeleteColumn(long boardId, string id);

        IEnumerable<ICard> GetCards(long boardId, string columnId);
        ICard GetCard(long boardId, string columnId, Guid id);
        void AddCard(long boardId, string columnId, ICard card);
        void DeleteCard(long boardId, string columnId, Guid id);
    }
}