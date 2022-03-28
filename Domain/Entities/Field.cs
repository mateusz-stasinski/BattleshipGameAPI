using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Field
    {
        public Guid Id { get; set; }
        public int X_Position { get; set; }
        public int Y_Position { get; set; }
        public FieldStatus Status { get; set; }

        public int? ShipId { get; set; }

        public int BoardId { get; set; }
    }

    public enum FieldStatus
    {
        Empty = 0,
        Filled = 1,
        Hit = 3,
        HitAndSink = 4
    }
}
