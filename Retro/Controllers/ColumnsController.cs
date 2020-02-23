using System;
using Microsoft.AspNetCore.Mvc;
using Retro.Exceptions;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Controllers
{
    [ApiController]
    [Route("boards/{boardId}/columns")]
    [Produces("application/vnd.api+json")]
    public class ColumnsController : RetroControllerBase
    {
        private readonly IColumnService _service;

        public ColumnsController(IColumnService service)
        {
            _service = service;
        }

        [HttpGet]
        [HttpGet("{id}")]
        public ActionResult<object> Get(long boardId, string id = null)
        {
            try
            {
                if (id == null)
                    return _service.GetColumns(boardId);
                return (Column) _service.GetColumn(boardId, id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public ActionResult<Column> Create(long boardId, Column column)
        {
            try
            {
                _service.AddColumn(boardId, column);
                return column;
            }
            catch (BoardNotFoundException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}