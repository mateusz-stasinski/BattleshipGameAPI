using BattleshipGameAPI.Models;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleshipGameAPI.Services
{
    public class GameService : IGameService
    {
        private readonly BattleshipGameDbContext _context;
        public GameService(BattleshipGameDbContext context)
        {
            _context = context;
        }

        public async Task<GameDto> ContinueGame(int gameId)
        {
            var game = await _context.Games
                .Include(g => g.Players)
                .ThenInclude(p => p.Ships)
                .Include(g => g.Players)
                .ThenInclude(p => p.Board)
                .ThenInclude(b => b.Fields.OrderBy(f => f.Y_Position).ThenBy(f => f.X_Position))
                .SingleOrDefaultAsync(g => g.Id == gameId);

            var gameDto = new GameDto();

            gameDto.Id = gameId;
            gameDto.IsEnded = game.IsEnded;
            gameDto.WinnerId = game.WinnerId;
            gameDto.Players = new List<PlayerDto>();
            foreach (var player in game.Players)
            {
                var playerDto = new PlayerDto()
                {
                    Id = player.Id,
                    Name = player.Name,
                    Score = player.Score,
                    IsWinner = player.IsWinner,
                    Board = new BoardDto()
                    {
                        Id = player.Board.Id,
                        PlayerId = player.Id,
                        X_Size = player.Board.X_Size,
                        Y_Size = player.Board.Y_Size,
                        Fields = new List<FieldDto>()
                    },
                    Ships = new List<ShipDto>()
                };

                foreach (var field in player.Board.Fields)
                {
                    playerDto.Board.Fields.Add(new FieldDto
                    {
                        X_Position = field.X_Position,
                        Y_Position = field.Y_Position,
                        Status = field.Status,
                        ShipId = field.ShipId,
                        BoardId = field.BoardId
                    });
                }

                foreach (var ship in player.Ships)
                {
                    var shipDto = new ShipDto()                    
                    {
                        Id = ship.Id,
                        Name = ship.Name,
                        Length = ship.Length,
                        PlayerId = ship.PlayerId,
                        Fields = new List<FieldDto>()
                    };

                    foreach (var field in ship.Fields)
                    {
                        shipDto.Fields.Add(new FieldDto
                        {
                            X_Position = field.X_Position,
                            Y_Position = field.Y_Position,
                            Status = field.Status,
                            ShipId = field.ShipId,
                            BoardId = field.BoardId
                        });
                    }

                    playerDto.Ships.Add(shipDto);
                }

                gameDto.Players.Add(playerDto);
            }

            return gameDto;
        }

        public async Task<GameDto> StartNewGame(StartNewGameRequest request)
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

            var gameDto = await ContinueGame(game.Id);

            return gameDto;
        }
    }
}
