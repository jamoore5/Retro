using System.Collections.Generic;
using Retro.Interfaces;

namespace Retro.Services
{
    public class RetroService : IRetroService
    {
        private IBoardService _boardService;
        private IColumnService _columnService;

        public RetroService(IBoardService boardService, IColumnService columnService)
        {
            _boardService = boardService;
            _columnService = columnService;
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
            _columnService.DeleteColumns(id);
            _boardService.DeleteBoard(id);
        }

        public void DeleteColumn(long boardId, string id)
        {
            _columnService.DeleteColumn(boardId, id);
        }
    }
}