using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Services
{
    public class ColumnService
    {
        private readonly IEnumerable<IColumn> _columns;
        private readonly BoardService _boardService;

        public ColumnService(BoardService boardService)
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

        public List<IColumn> GetColumn(long boardId, string id)
        {
            var columns = GetColumns(boardId)
                .Where(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase)).ToList();
            return columns.Any() ? columns : null;
        }


    }
}