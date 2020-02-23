using System.Collections.Generic;
using Retro.Models;

namespace Retro.Interfaces
{
    public interface IColumnService
    {
        IEnumerable<IColumn> GetColumns(long boardId);

        IColumn GetColumn(long boardId, string id);
        void AddColumn(long boardId, IColumn column);
        void DeleteColumns(long boardId);

        void DeleteColumn(long boardId, string id);
    }
}