using System;

namespace Retro.Interfaces
{
    public interface ICard
    {
        long BoardId { get; set; }
        string ColumnId { get; set; }
        Guid Id { get; set; }
        string Text { get; set; }
    }
}