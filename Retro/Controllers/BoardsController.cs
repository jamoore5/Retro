using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/vnd.api+json")]
    public class BoardsController
    {
        [HttpGet]
        public IEnumerable<IBoard> Get()
        {
            return new List<IBoard>
            {
                new Board{Name = "Start"},
                new Board{Name = "Stop"},
                new Board{Name = "Continue"}
            };
        }
    }
}