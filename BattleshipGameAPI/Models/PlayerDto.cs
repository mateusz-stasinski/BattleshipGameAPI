using System.Collections.Generic;

namespace BattleshipGameAPI.Models
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public bool? IsWinner { get; set; }
        public bool IsMyOpponentMove { get; set; }
        public BoardDto Board { get; set; }
        public ICollection<ShipDto> Ships { get; set; }
    }
}
