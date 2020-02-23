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
        private readonly IBoardService _service;

        public BoardsController(IBoardService service)
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