using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/vnd.api+json")]
    public class BoardsController : ControllerBase
    {
        private IEnumerable<IBoard> _boards;


        public BoardsController()
        {
            _boards = new List<IBoard>
            {
                new Board{Id = "Start", Name = "Start"},
                new Board{Id = "Stop", Name = "Stop"},
                new Board{Id = "Continue", Name = "Continue"}
            };
        }

        [HttpGet]
        public IEnumerable<IBoard> Get()
        {
            return _boards;
        }

        [HttpGet("{id}")]
        public ActionResult<Board> Get(string id)
        {
            var board = _boards.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (board == null) return NotFound();
            return (Board) board;
        }
    }
}