using System.Collections.Generic;

namespace BattleshipGameAPI.Models
{
    public class BoardDto
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int X_Size { get; set; }
        public int Y_Size { get; set; }

        public ICollection<BoardRow> Rows { get; set; }
    }

    public class BoardRow
    {
        public ICollection<FieldDto> Fields { get; set; }
    }
}
