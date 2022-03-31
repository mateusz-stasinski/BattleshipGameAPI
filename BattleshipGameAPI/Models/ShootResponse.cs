namespace BattleshipGameAPI.Models
{
    public class ShootResponse
    {
        public bool IsHit { get; set; }
        public bool IsSunk { get; set; }
        public bool IsGameOver { get; set; }
    }
}
