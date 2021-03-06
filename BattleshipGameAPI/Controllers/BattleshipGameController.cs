using BattleshipGameAPI.Models;
using BattleshipGameAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BattleshipGameAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BattleshipGameController : ControllerBase
    {
        private readonly IGameService _service;
        public BattleshipGameController(IGameService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ContinueGame([FromRoute] int id)
        {
            var game = await _service.ContinueGame(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> StartNewGame([FromBody] StartNewGameRequest request)
        {
            var game = await _service.StartNewGame(request);

            return Ok(game);
        }

        [HttpPut]
        public async Task<IActionResult> Shoot([FromBody] ShootRequest request)
        {
            var shot = await _service.Shoot(request);

            if (shot == null)
            {
                return Problem("It is not your move", "Player", 409);
            }

            return Ok(shot);
        }
    }
}
