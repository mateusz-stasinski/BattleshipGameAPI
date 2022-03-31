using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Ship
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public int Length { get; set; }

        public virtual ICollection<Field> Fields { get; set; }
        public bool IsSunk { get; set; }

        public int PlayerId { get; set; }
    }
}
