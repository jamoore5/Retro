using System;
using Microsoft.AspNetCore.Mvc;

namespace Retro.Controllers
{
    public abstract class RetroControllerBase : ControllerBase
    {
        protected BadRequestObjectResult BadRequest(Exception ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = ex.Message, Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            });
        }

        protected NotFoundObjectResult NotFound(Exception ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = ex.Message, Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            });
        }
    }
}