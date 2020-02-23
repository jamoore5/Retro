using System.Collections.Generic;

namespace Retro.Interfaces
{
    public interface IColumnService
    {
        List<IColumn> GetColumns(long boardId);

        IColumn GetColumn(long boardId, string id);
    }
}