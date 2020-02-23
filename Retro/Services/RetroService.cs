using System;
using System.Collections.Generic;
using Retro.Interfaces;

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


        public List<IBoard> GetBoards()
        {
            return _boardService.GetBoards();
        }

        public IBoard GetBoard(long id)
        {
            return _boardService.GetBoard(id);
        }

        public void AddBoard(IBoard board)
        {
            _boardService.AddBoard(board);
        }

        public List<IColumn> GetColumns(long boardId)
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