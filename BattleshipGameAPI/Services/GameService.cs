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
            var game = await GetGame(gameId);

            if (game == null)
            {
                return null;
            }

            var gameDto = MapGame(game);

            foreach (var player in game.Players)
            {
                var playerDto = MapPlayer(player);

                MapBoardForPlayer(player, playerDto);

                MapShipsForPlayer(player, playerDto);

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

        public async Task<ShootResponse> Shoot(ShootRequest request)
        {
            var game = await GetGame(request.GameId);

            var response = new ShootResponse();

            var attackedPlayer = game.Players.Single(p => p.Id == request.AttackedPlayerId);
            var board = attackedPlayer.Board;
            var field = board.Fields.Single(f => f.X_Position == request.X_Position && f.Y_Position == request.Y_Position);

            if (!attackedPlayer.IsMyOpponentMove)
            {
                return null;
            }

            attackedPlayer.IsMyOpponentMove = false;

            var shootingPlayer = game.Players.First(p => p.Id != request.AttackedPlayerId);
            shootingPlayer.IsMyOpponentMove = true;

            if (field.Status == FieldStatus.Empty  || field.Status == FieldStatus.Missed) 
            {
                field.Status = FieldStatus.Missed;
                response.IsHit = false;
                response.IsSunk = false;
                response.IsGameOver = game.IsEnded;
                response.Status = field.Status;
            }
            await _context.SaveChangesAsync();

            if (field.Status == FieldStatus.Filled)
            {
                shootingPlayer.Score++;
                field.Status = FieldStatus.Hit;
                var ship = attackedPlayer.Ships.Single(s => s.Id == field.ShipId);

                //Checking if the ship hit and sink 
                int hitFieldsCounter = 0;
                foreach (var shipField in ship.Fields)
                {
                    if (shipField.Status == FieldStatus.Hit)
                    {
                        hitFieldsCounter++;
                    }
                }

                //The ship is not sunk 
                if (hitFieldsCounter != ship.Fields.Count())
                {
                    response.IsHit = true;
                    response.IsSunk = ship.IsSunk;
                    response.IsGameOver = game.IsEnded;
                    response.Status = field.Status;
                    response.ShipId = field.ShipId;
                    response.BoardId = field.BoardId;
                }

                //The ship is sunk
                if (hitFieldsCounter == ship.Fields.Count())
                {
                    ship.IsSunk = true;


                    //Checking whether the game is over 
                    int sunkShipsCounter = 0;
                    foreach (var playerShip in attackedPlayer.Ships)
                    {
                        if (playerShip.IsSunk)
                        {
                            sunkShipsCounter++;
                        }
                    }

                    //Continuation of the game 
                    if (sunkShipsCounter != attackedPlayer.Ships.Count())
                    {
                        response.IsHit = true;
                        response.IsSunk = ship.IsSunk;
                        response.IsGameOver = game.IsEnded;
                        response.Status = field.Status;
                        response.ShipId = field.ShipId;
                        response.BoardId = field.BoardId;
                    }

                    //End of the game 
                    if (sunkShipsCounter == attackedPlayer.Ships.Count())
                    {
                        game.IsEnded = true;
                        shootingPlayer.IsWinner = true;
                        shootingPlayer.IsMyOpponentMove = false;
                        attackedPlayer.IsMyOpponentMove = false;
                       
                        response.IsHit = true;
                        response.IsSunk = ship.IsSunk;
                        response.IsGameOver = game.IsEnded;
                        response.Status = field.Status;
                        response.ShipId = field.ShipId;
                        response.BoardId = field.BoardId;
                    }
                }

                await _context.SaveChangesAsync();
            }
            return response;
        }

        private async Task<Game> GetGame(int gameId)
        {
            var game =  await _context.Games
                .Include(g => g.Players)
                .ThenInclude(p => p.Ships)
                .Include(g => g.Players)
                .ThenInclude(p => p.Board)
                .ThenInclude(b => b.Fields.OrderBy(f => f.Y_Position).ThenBy(f => f.X_Position))
                .SingleOrDefaultAsync(g => g.Id == gameId);
            if (game == null)
            {
                return null;
            }

            return game;
        }

        private GameDto MapGame(Game game)
        {
            var gameDto = new GameDto();
            gameDto.Id = game.Id;
            gameDto.IsEnded = game.IsEnded;
            gameDto.WinnerId = game.WinnerId;
            gameDto.Players = new List<PlayerDto>();
            return gameDto;
        }

        private PlayerDto MapPlayer(Player player)
        {
            var playerDto = new PlayerDto()
            {
                Id = player.Id,
                Name = player.Name,
                Score = player.Score,
                IsWinner = player.IsWinner,
                IsMyOpponentMove = player.IsMyOpponentMove,
                Board = new BoardDto()
                {
                    Id = player.Board.Id,
                    PlayerId = player.Id,
                    X_Size = player.Board.X_Size,
                    Y_Size = player.Board.Y_Size,
                    Rows = new List<BoardRow>()
                },
                Ships = new List<ShipDto>()
            };
            return playerDto;
        }

        private void MapBoardForPlayer(Player player, PlayerDto playerDto)
        {
            for (int i = 0; i < player.Board.Y_Size; i++)
            {
                var row = new BoardRow();
                row.Fields = new List<FieldDto>();

                for (int j = 0; j < player.Board.X_Size; j++)
                {
                    var field = player.Board.Fields.Single(f => f.X_Position == j + 1 && f.Y_Position == i + 1);

                    row.Fields.Add(new FieldDto
                    {
                        X_Position = field.X_Position,
                        Y_Position = field.Y_Position,
                        Status = field.Status,
                        ShipId = field.ShipId,
                        BoardId = field.BoardId
                    });
                }
                playerDto.Board.Rows.Add(row);
            }
        }

        private void MapShipsForPlayer(Player player, PlayerDto playerDto)
        {
            foreach (var ship in player.Ships)
            {
                var shipDto = new ShipDto()
                {
                    Id = ship.Id,
                    Name = ship.Name,
                    Length = ship.Length,
                    PlayerId = ship.PlayerId,
                    IsSunk = ship.IsSunk,
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
        }
    }
}
