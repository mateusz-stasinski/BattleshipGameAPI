using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Player
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public int Score { get; set; }
        public bool? IsWinner { get; set; }

        public int GameId { get; set; }

        public virtual Board Board { get; set; }
        public virtual ICollection<Ship> Ships { get; set; }
    }
}
