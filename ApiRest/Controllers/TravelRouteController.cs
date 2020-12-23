using System.Net;
using Domain.Interfaces;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiRest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelRouteController : ControllerBase
    {
        private readonly ITravelRouteService _travelRouteService;

        public TravelRouteController(ITravelRouteService travelRouteService)
        {
            _travelRouteService = travelRouteService;
        }

        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult AddRoute([FromBody] NewRoute newRoute)
        {
            string error = _travelRouteService.AddRoute(newRoute);

            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);

            return Ok();
        }

        [HttpGet("[action]/From/{from}/To/{to}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BestRoute))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BestRoute))]
        public IActionResult CalculateBestRoute([FromRoute] string from, [FromRoute] string to)
        {
            BestRoute bestRoute = _travelRouteService.CalculateBestRoute(from, to);

            if (!string.IsNullOrEmpty(bestRoute.Error))
                return BadRequest(bestRoute);

            return Ok(bestRoute);
        }
    }
}