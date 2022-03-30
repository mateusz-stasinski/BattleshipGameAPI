namespace BattleshipGameAPI.Models
{
    public class FieldDto
    {
        public int Y_Position { get; set; }
        public int X_Position { get; set; }
        public Domain.Entities.FieldStatus Status { get; set; }

        public int? ShipId { get; set; }

        public int BoardId { get; set; }
    }
}
