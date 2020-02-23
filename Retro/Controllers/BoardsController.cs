using System.Net;
using Microsoft.AspNetCore.Mvc;
using Retro.Exceptions;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/vnd.api+json")]
    public class BoardsController : RetroControllerBase
    {
        private readonly IRetroService _service;

        public BoardsController(IRetroService service)
        {
            _service = service;
        }

        [HttpGet]
        [HttpGet("{id}")]
        public ActionResult<object> Get(long id = 0)
        {
            if (id == 0)
                return _service.GetBoards();

            try
            {
                return (Board) _service.GetBoard(id);
            }
            catch (BoardNotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public ActionResult<Board> Create(Board board)
        {
            try
            {
                _service.AddBoard(board);
                return board;
            }
            catch (UnsupportedClientGeneratedIdException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            try
            {
                _service.DeleteBoard(id);
                return Ok();
            }
            catch (BoardNotFoundException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}