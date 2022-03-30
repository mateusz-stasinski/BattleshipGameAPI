using BattleshipGameAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BattleshipGameAPI.Services
{
    public interface IGameService
    {
        public Task<GameDto> StartNewGame(StartNewGameRequest request);
        public Task<GameDto> ContinueGame(int gameId);
        public Task<PlayerDto> Shoot(ShootRequest request);
    }
}
