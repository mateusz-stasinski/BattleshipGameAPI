using System.Collections.Generic;

namespace BattleshipGameAPI.Models
{
    public class GameDto
    {
        public int Id { get; set; }
        public bool IsEnded { get; set; }
        public int? WinnerId { get; set; }
        public ICollection<PlayerDto> Players { get; set; }
    }
}
