using System;
using System.Collections.Generic;
using System.Linq;
using Retro.Exceptions;
using Retro.Interfaces;

namespace Retro.Services
{
    public class CardService : ICardService
    {
        private List<ICard> _cards;
        private IColumnService _columnService;

        public CardService(IColumnService columnService)
        {
            _cards = new List<ICard>();
            _columnService = columnService;
        }

        public IEnumerable<ICard> GetCards(long boardId, string columnId)
        {
            var cards = _cards.Where(x => x.BoardId == boardId && x.ColumnId == columnId).ToList();
            if (cards.Any()) return cards;

            _columnService.GetColumn(boardId, columnId);
            return new List<ICard>();
        }

        public ICard GetCard(long boardId, string columnId, Guid id)
        {
            var card = _cards.FirstOrDefault(x => x.BoardId == boardId && x.ColumnId == columnId && x.Id == id);
            if (card == null) throw new CardNotFoundException(id);
            return card;
        }

        public void AddCard(long boardId, string columnId, ICard card)
        {
            if (card.Id != Guid.Empty)
                throw new UnsupportedClientGeneratedIdException();

            _columnService.GetColumn(boardId, columnId);

            card.BoardId = boardId;
            card.ColumnId = columnId;
            card.Id = Guid.NewGuid();

            _cards.Add(card);
        }

        public void DeleteCard(long boardId, string columnId, Guid id)
        {
            _cards.Remove(GetCard(boardId, columnId, id));
        }

        public void DeleteCards(long boardId)
        {
            _cards.RemoveAll(x => x.BoardId == boardId);
        }
    }
}