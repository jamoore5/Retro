using System.Collections.Generic;
using Retro.Models;

namespace Retro.Interfaces
{
    public interface IColumnService
    {
        List<IColumn> GetColumns(long boardId);

        IColumn GetColumn(long boardId, string id);
        void AddColumn(long boardId, IColumn column);
    }
}