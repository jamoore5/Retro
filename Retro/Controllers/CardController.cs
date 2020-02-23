using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Retro.Exceptions;
using Retro.Interfaces;
using Retro.Models;

namespace Retro.Controllers
{
    [ApiController]
    [Route("boards/{boardId}/columns/{columnId}/cards")]
    [Produces("application/vnd.api+json")]
    public class CardController : RetroControllerBase
    {
        private readonly ICardService _service;

        public CardController(ICardService service)
        {
            _service = service;
        }

        [HttpGet]
        [HttpGet("{id}")]
        public ActionResult<object> Get(long boardId, string columnId, Guid? id = null)
        {
            try
            {
                if (id == null)
                    return (List<ICard>) _service.GetCards(boardId, columnId);
                return (Card) _service.GetCard(boardId, columnId, id.Value);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public ActionResult<Card> Create(long boardId, string columnId, Card card)
        {
            try
            {
                _service.AddCard(boardId, columnId, card);
                return (Card) card;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException || ex is UnsupportedClientGeneratedIdException)
                    return BadRequest(ex);

                throw;
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(long boardId, string columnId, Guid id)
        {
            try
            {
                _service.DeleteCard(boardId, columnId, id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex);
            }
        }

    }
}