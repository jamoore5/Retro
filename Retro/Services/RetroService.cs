using System;
using System.Collections.Generic;
using System.Linq;
using Retro.Exceptions;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Services
{
    public class RetroService : IRetroService
    {
        private readonly IBoardService _boardService;
        private readonly IColumnService _columnService;
        private readonly ICardService _cardService;

        public RetroService(IBoardService boardService, IColumnService columnService, ICardService cardService)
        {
            _boardService = boardService;
            _columnService = columnService;
            _cardService = cardService;
        }


        public IEnumerable<IBoard> GetBoards()
        {
            return _boardService.GetBoards().ToList();
        }

        public IBoard GetBoard(long id, bool includeColumns = false, bool includeCards = false)
        {
            if (includeCards && !includeColumns)
                throw new InvalidIncludeException();

            var board = _boardService.GetBoard(id).Clone();

            if (includeColumns)
            {
                var columns = _columnService.GetColumns(id).Select(x => x.Clone()).ToList();
                if (includeCards)
                {
                    foreach (var column in columns)
                    {
                        column.Cards = _cardService.GetCards(id, column.Id).Select(x => (Card)x);
                    }
                }
                board.Columns = columns;
            }

            return board;
        }

        public void AddBoard(IBoard board)
        {
            _boardService.AddBoard(board);
        }

        public IEnumerable<IColumn> GetColumns(long boardId)
        {
            return _columnService.GetColumns(boardId);
        }

        public IColumn GetColumn(long boardId, string id)
        {
            return _columnService.GetColumn(boardId, id);
        }

        public void AddColumn(long boardId, IColumn column)
        {
            _columnService.AddColumn(boardId, column);
        }

        public void DeleteBoard(long id)
        {
            // TODO make transactional
            _cardService.DeleteCards(id);
            _columnService.DeleteColumns(id);
            _boardService.DeleteBoard(id);
        }

        public void DeleteColumn(long boardId, string id)
        {
            _columnService.DeleteColumn(boardId, id);
        }

        public IEnumerable<ICard> GetCards(long boardId, string columnId)
        {
            return _cardService.GetCards(boardId, columnId);
        }

        public ICard GetCard(long boardId, string columnId, Guid id)
        {
            return _cardService.GetCard(boardId, columnId, id);
        }

        public void AddCard(long boardId, string columnId, ICard card)
        {
            _cardService.AddCard(boardId, columnId, card);
        }

        public void DeleteCard(long boardId, string columnId, Guid id)
        {
            _cardService.DeleteCard(boardId, columnId, id);
        }
    }
}