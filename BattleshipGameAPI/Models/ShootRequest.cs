namespace BattleshipGameAPI.Models
{
    public class ShootRequest
    {
        public int GameId { get; set; }
        public int PlayerWhoIsShootingId { get; set; }
        public int AttackedPlayerId { get; set; }
        public int BoardId { get; set; }
        public int Y_Position { get; set; }
        public int X_Position { get; set; }
    }
}
