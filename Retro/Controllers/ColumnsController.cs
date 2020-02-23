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
        private readonly IColumnService _service;

        public ColumnsController(IColumnService service)
        {
            _service = service;
        }

        [HttpGet()]
        [HttpGet("{id}")]
        public ActionResult<object> Get(long boardId, string id = null)
        {
            object data = (id == null) ? _service.GetColumns(boardId) : _service.GetColumn(boardId, id);
            if (data == null) return NotFound();
            return data;
        }
    }
}