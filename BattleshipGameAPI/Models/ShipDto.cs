using System.Collections.Generic;

namespace BattleshipGameAPI.Models
{
    public class ShipDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public ICollection<FieldDto> Fields { get; set; }
        public bool IsSunk { get; set; }
        public int PlayerId { get; set; }
    }
}
