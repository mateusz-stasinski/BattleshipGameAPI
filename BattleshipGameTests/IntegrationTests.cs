using BattleshipGameAPI.Controllers;
using BattleshipGameAPI.Models;
using BattleshipGameAPI.Services;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BattleshipGameTests
{
    public class IntegrationTests : IDisposable
    {
        public string ConnectionString =>
            //"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BattleshipGame_TestDB;Integrated Security=True";
            "Server=(localdb)\\mssqllocaldb;Database=BattleshipGame_TestDB;Trusted_Connection=True";

        private readonly BattleshipGameDbContext _context;
        private readonly BattleshipGameController _controller;
        private readonly GameService _service;

        public IntegrationTests()
        {
            var builder = new DbContextOptionsBuilder<BattleshipGameDbContext>();
            builder.UseSqlServer(ConnectionString);
            _context = new BattleshipGameDbContext(builder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.Migrate();

            _service = new GameService(CreateDbContext());
            _controller = new BattleshipGameController(_service);
        }

        [Fact]
        public async Task ShouldStartNewGame()
        {
            //Arrange
            var request = new StartNewGameRequest
            {
                FirstPlayerName = GenerateString(),
                SecondPlayerName = GenerateString(),
                XSize = 10,
                YSize = 10,
            };

            var gameActionResult = await _controller.StartNewGame(request);
            var gameResult = gameActionResult as OkObjectResult;
            var game = (GameDto)gameResult.Value;

            //Act
            Game addedGame;
            using (var context = CreateDbContext())
            {
                addedGame = await context.Games
                    .Include(g => g.Players)
                    .ThenInclude(p => p.Board)
                    .SingleOrDefaultAsync(g => g.Id == game.Id);
            }


            //Assert
            Assert.NotNull(addedGame);
            Assert.Equal(game.Id, addedGame.Id);
            Assert.Contains(addedGame.Players, p => p.Name == request.FirstPlayerName);
            Assert.Contains(addedGame.Players, p => p.Name == request.SecondPlayerName);
            foreach (var player in addedGame.Players)
            {
                Assert.Equal(request.XSize, player.Board.X_Size);
                Assert.Equal(request.YSize, player.Board.Y_Size);
            }
        }

        [Fact]
        public async Task ShouldContinueGame()
        {
            //Arrange
            var request = new StartNewGameRequest
            {
                FirstPlayerName = GenerateString(),
                SecondPlayerName = GenerateString(),
                XSize = 10,
                YSize = 10,
            };

            var gameActionResult = await _controller.StartNewGame(request);
            var gameResult = gameActionResult as OkObjectResult;
            var game = (GameDto)gameResult.Value;

            //Act
            var continuationGameActionResult = _controller.ContinueGame(game.Id);
            var continuationGameResult = continuationGameActionResult.Result as OkObjectResult;
            var continuationGame = (GameDto)continuationGameResult.Value;

            //Assert
            Assert.NotNull(continuationGame);
            Assert.Equal(game.Id, continuationGame.Id);
            Assert.Contains(continuationGame.Players, p => p.Name == request.FirstPlayerName);
            Assert.Contains(continuationGame.Players, p => p.Name == request.SecondPlayerName);
            foreach (var player in continuationGame.Players)
            {
                Assert.Equal(request.XSize, player.Board.X_Size);
                Assert.Equal(request.YSize, player.Board.Y_Size);
            }
        }

        [Fact]
        public async Task ShouldNotShootIfNotYourTurn()
        {
            //Arrange
            var request = new StartNewGameRequest
            {
                FirstPlayerName = GenerateString(),
                SecondPlayerName = GenerateString(),
                XSize = 10,
                YSize = 10,
            };

            var gameActionResult = await _controller.StartNewGame(request);
            var gameResult = gameActionResult as OkObjectResult;
            var game = (GameDto)gameResult.Value;

            var player = game.Players.Single(g => g.IsMyOpponentMove == false);
            FieldDto field = null;

            while (field == null)
            {
                int x = new Random().Next(0, request.XSize);
                int y = new Random().Next(0, request.YSize);
                var randomfield = player.Board.Rows.ElementAt(x).Fields.ElementAt(y);

                if (randomfield.Status == FieldStatus.Empty)
                {
                    field = randomfield;
                }
            }

            var shootRequest = new ShootRequest
            {
                GameId = game.Id,
                AttackedPlayerId = player.Id,
                Y_Position = field.Y_Position,
                X_Position = field.X_Position,
            };
            var response = await _service.Shoot(shootRequest);

            //Act
            Game addedGame;
            using (var context = CreateDbContext())
            {
                addedGame = await context.Games
                    .Include(g => g.Players)
                    .ThenInclude(p => p.Board)
                    .ThenInclude(b => b.Fields)
                    .Include(g => g.Players)
                    .ThenInclude(p => p.Ships)
                    .SingleAsync(g => g.Id == game.Id);

            }

            var attackedPlayer = addedGame.Players.Single(p => p.Id == player.Id);
            var shootingPlayer = addedGame.Players.First(p => p.Id != player.Id);
            var shootField = attackedPlayer.Board.Fields
                .Single(f => f.X_Position == field.X_Position && f.Y_Position == field.Y_Position);

            //Assert
            Assert.Null(response);
            Assert.Equal(FieldStatus.Empty, shootField.Status);
            Assert.Equal(0, attackedPlayer.Score);
            Assert.Equal(0, shootingPlayer.Score);
        }

        [Fact]
        public async Task ShouldShootIfEmptyField()
        {
            //Arrange
            var request = new StartNewGameRequest
            {
                FirstPlayerName = GenerateString(),
                SecondPlayerName = GenerateString(),
                XSize = 10,
                YSize = 10,
            };

            var gameActionResult = await _controller.StartNewGame(request);
            var gameResult = gameActionResult as OkObjectResult;
            var game = (GameDto)gameResult.Value;

            var player = game.Players.Single(g => g.IsMyOpponentMove == true);
            FieldDto field = null;

            while (field == null)
            {
                int x = new Random().Next(0, request.XSize);
                int y = new Random().Next(0, request.YSize);
                var randomfield = player.Board.Rows.ElementAt(x).Fields.ElementAt(y);

                if (randomfield.Status == FieldStatus.Empty)
                {
                    field = randomfield;
                }
            }

            var shootRequest = new ShootRequest
            {
                GameId = game.Id,
                AttackedPlayerId = player.Id,
                Y_Position = field.Y_Position,
                X_Position = field.X_Position,
            };
            await _service.Shoot(shootRequest);

            //Act
            Game addedGame;
            using (var context = CreateDbContext())
            {
                addedGame = await context.Games
                    .Include(g => g.Players)
                    .ThenInclude(p => p.Board)
                    .ThenInclude(b => b.Fields)
                    .Include(g => g.Players)
                    .ThenInclude(p => p.Ships)
                    .SingleAsync(g => g.Id == game.Id);

            }

            var attackedPlayer = addedGame.Players.Single(p => p.Id == player.Id);
            var shootingPlayer = addedGame.Players.First(p => p.Id != player.Id);
            var shootField = attackedPlayer.Board.Fields
                .Single(f => f.X_Position == field.X_Position && f.Y_Position == field.Y_Position);

            //Assert
            Assert.NotNull(shootField);
            Assert.Equal(FieldStatus.Missed, shootField.Status);
            Assert.Equal(0, attackedPlayer.Score);
            Assert.Equal(0, shootingPlayer.Score);
        }

        [Fact]
        public async Task ShouldShootIfFilledField()
        {
            //Arrange
            var request = new StartNewGameRequest
            {
                FirstPlayerName = GenerateString(),
                SecondPlayerName = GenerateString(),
                XSize = 10,
                YSize = 10,
            };

            var gameActionResult = await _controller.StartNewGame(request);
            var gameResult = gameActionResult as OkObjectResult;
            var game = (GameDto)gameResult.Value;

            var player = game.Players.Single(g => g.IsMyOpponentMove == true);
            FieldDto field = null;

            while (field == null)
            {
                int x = new Random().Next(0, request.XSize);
                int y = new Random().Next(0, request.YSize);
                var randomfield = player.Board.Rows.ElementAt(x).Fields.ElementAt(y);

                if (randomfield.Status == FieldStatus.Filled)
                {
                    field = randomfield;
                }
            }

            var shootRequest = new ShootRequest
            {
                GameId = game.Id,
                AttackedPlayerId = player.Id,
                Y_Position = field.Y_Position,
                X_Position = field.X_Position,
            };
            var response = await _service.Shoot(shootRequest);

            //Act
            Game addedGame;
            using (var context = CreateDbContext())
            {
                addedGame = await context.Games
                    .Include(g => g.Players)
                    .ThenInclude(p => p.Board)
                    .ThenInclude(b => b.Fields)
                    .Include(g => g.Players)
                    .ThenInclude(p => p.Ships)
                    .SingleAsync(g => g.Id == game.Id);

            }

            var attackedPlayer = addedGame.Players.Single(p => p.Id == player.Id);
            var shootingPlayer = addedGame.Players.First(p => p.Id != player.Id);
            var shootField = attackedPlayer.Board.Fields
                .Single(f => f.X_Position == field.X_Position && f.Y_Position == field.Y_Position);

            //Assert
            Assert.NotNull(shootField);
            Assert.Equal(FieldStatus.Hit, shootField.Status);
            Assert.Equal(0, attackedPlayer.Score);
            Assert.Equal(1, shootingPlayer.Score);
            Assert.Equal(response.ShipId, shootField.ShipId);
        }

        private string GenerateString()
        {
            return Guid.NewGuid().ToString().Substring(0, 11);
        }

        private BattleshipGameDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BattleshipGameDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            return new BattleshipGameDbContext(optionsBuilder.Options);
        }

        public void Dispose()
        {
        }
    }
}
