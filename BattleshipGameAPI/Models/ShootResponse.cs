namespace BattleshipGameAPI.Models
{
    public class ShootResponse
    {
        public bool IsHit { get; set; }
        public bool IsSunk { get; set; }
        public bool IsGameOver { get; set; }
        public Domain.Entities.FieldStatus Status { get; set; }
        public int? ShipId { get; set; }
        public int BoardId { get; set; }
    }
}
