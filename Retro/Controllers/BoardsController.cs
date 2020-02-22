using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Retro.Interfaces;
using Retro.Models;
using Retro.Services;

namespace Retro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/vnd.api+json")]
    public class BoardsController : ControllerBase
    {
        private readonly BoardService _service;

        public BoardsController(BoardService service)
        {
            _service = service;
        }

        [HttpGet]
        [HttpGet("{id}")]
        public ActionResult<object> Get(long id = 0)
        {
            if (id == 0)
                return _service.GetBoards();

            var board = _service.GetBoard(id);
            if (board == null) return NotFound();
            return (Board) board;
        }
    }
}