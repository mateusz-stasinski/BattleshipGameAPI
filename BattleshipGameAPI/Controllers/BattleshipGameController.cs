using BattleshipGameAPI.Models;
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
        private readonly BattleshipGameDbContext _context;
        public BattleshipGameController(BattleshipGameDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ContinueGame([FromRoute] int id)
        {
            var game = await _context.Games
                .Include(g => g.Players)
                .ThenInclude(p => p.Ships)
                .Include(g => g.Players)
                .ThenInclude(p => p.Board)
                .ThenInclude(b => b.Fields.OrderBy(f => f.Y_Position).ThenBy(f => f.X_Position))
                .SingleOrDefaultAsync(g => g.Id == id);

            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> StartNewGame([FromBody] StartNewGameRequest request)
        {
            var game = new Game();
            game.StartNewGame(request.FirstPlayerName, request.SecondPlayerName, request.XSize, request.YSize);

            _context.Add(game);
            await _context.SaveChangesAsync();

            var players = game.Players.ToList();

            foreach (var player in players)
            {
                var ships = await _context.Ships.Where(s => s.PlayerId == player.Id).ToListAsync();
                var board = await _context.Boards.Include(b => b.Fields).SingleAsync(b => b.PlayerId == player.Id);

                foreach (var ship in ships)
                {
                    game.PlaceShip(ship, board);
                }
            }
            await _context.SaveChangesAsync();

            return Created($"/api/BattleshipGame/{game.Id}", game.Id);
        }
    }
}
