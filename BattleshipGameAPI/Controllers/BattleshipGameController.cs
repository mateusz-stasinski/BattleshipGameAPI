using BattleshipGameAPI.Models;
using BattleshipGameAPI.Services;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> StartNewGame([FromBody] StartNewGameRequest request)
        {
            var game = await _service.StartNewGame(request);

            return Ok(game);
        }
    }
}
