using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<object> Get()
        {
            return _service.GetBoards().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<object> Get(long id, string include = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(include))
                    return (Board) _service.GetBoard(id);

                var includeList = include.Split(',').ToList();
                var includeColumns = includeList.Exists(IncludeColumns);
                var includeCards = includeList.Exists(IncludeCards);

                if (includeCards && !includeColumns)
                    throw new InvalidIncludeException();

                return (Board) _service.GetBoard(id, includeColumns, includeCards);
            }
            catch (BoardNotFoundException ex)
            {
                return NotFound(ex);
            }
            catch (InvalidIncludeException ex)
            {
                return BadRequest(ex);
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

        private static bool InvalidInclude(string x)
        {
            return !IncludeColumns(x) && !IncludeCards(x);
        }

        private static bool IncludeColumns(string x)
        {
            return x.Equals("columns", StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IncludeCards(string x)
        {
            return x.Equals("cards", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}