using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retro.Interfaces;
using Retro.Models;
using Retro.Services;

namespace Retro.Controllers
{
    [ApiController]
    [Route("boards/{boardId}/columns")]
    [Produces("application/vnd.api+json")]
    public class ColumnsController : ControllerBase
    {
        private readonly ColumnService _service;


        public ColumnsController(ColumnService service)
        {
            _service = service;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<IColumn>> Get(long boardId)
        {

            var columns = _service.GetColumns(boardId);
            if (columns == null) return NotFound();
            return columns;
        }

        [HttpGet("{id}")]
        public ActionResult<IColumn> Get(long boardId, string id)
        {
            var column = _service.GetColumns(boardId)?.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (column != null) return (Column) column;
            return NotFound();
        }
    }
}