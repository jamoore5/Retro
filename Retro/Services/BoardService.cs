using System;
using System.Collections.Generic;
using System.Linq;
using Retro.Exceptions;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Services
{
    public class BoardService : IBoardService
    {
        private List<IBoard> _boards;
        private long _lastId = 3;

        public BoardService()
        {
            _boards = new List<IBoard>
            {
                new Board{Id = 1, Name = "Sprint 1"},
                new Board{Id = 2, Name = "Sprint 2"},
                new Board{Id = 3, Name = "Sprint 3"}
            };
        }

        public List<IBoard> GetBoards()
        {
            return _boards.ToList();
        }

        public IBoard GetBoard(long id)
        {
            var board = _boards.FirstOrDefault(x => x.Id == id);
            if (board == null) throw new BoardNotFoundException(id);
            return board;
        }

        public void AddBoard(IBoard board)
        {
            if (board.Id != 0)
                throw new UnsupportedClientGeneratedIdException();

            board.Id = ++_lastId;
            _boards.Add(board);
        }
    }
}