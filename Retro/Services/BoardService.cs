using System.Collections.Generic;
using System.Linq;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Services
{
    public class BoardService
    {
        private IEnumerable<IBoard> _boards;

        public BoardService()
        {
            _boards = new List<IBoard>
            {
                new Board{Id = 1, Name = "Sprint 1"},
                new Board{Id = 2, Name = "Sprint 2"},
                new Board{Id = 3, Name = "Sprint 3"}
            };
        }

        public IEnumerable<IBoard> GetBoards()
        {
            return _boards;
        }

        public IBoard GetBoard(long id)
        {
            return _boards.FirstOrDefault(x => x.Id == id);
        }
    }
}