using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retro.Exceptions;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Services
{
    public class ColumnService : IColumnService
    {
        private readonly List<IColumn> _columns;
        private readonly IBoardService _boardService;

        public ColumnService(IBoardService boardService)
        {
            _columns = new List<IColumn>{
                new Column{Id = "Start", BoardId = 1, Name = "Start"},
                new Column{Id = "Stop", BoardId = 1, Name = "Stop"},
                new Column{Id = "Continue", BoardId = 1, Name = "Continue"}
            };

            _boardService = boardService;
        }
        public List<IColumn> GetColumns(long boardId)
        {
            var columns = _columns.Where(x => x.BoardId == boardId).ToList();
            if (columns.Any()) return columns;

            var board = _boardService.GetBoard(boardId);
            return board == null ? null : new List<IColumn>();
        }

        public IColumn GetColumn(long boardId, string id)
        {
            var column =  GetColumns(boardId).FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (column == null) throw new ColumnNotFoundException(id);
            return column;
        }

        public void AddColumn(long boardId, IColumn column)
        {
            _boardService.GetBoard(boardId);

            if (string.IsNullOrWhiteSpace(column.Id))
                column.Id = Guid.NewGuid().ToString();

            column.BoardId = boardId;

            _columns.Add(column);
        }
    }
}