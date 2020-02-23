using System.Collections.Generic;

namespace Retro.Interfaces
{
    public interface IBoardService
    {
        List<IBoard> GetBoards();

        IBoard GetBoard(long id);
        void AddBoard(IBoard board);
    }
}