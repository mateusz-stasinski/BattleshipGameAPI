using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Board
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int X_Size { get; set; }
        public int Y_Size { get; set; }

        public virtual ICollection<Field> Fields { get; set; }

        public void InitializeFields(int xSize, int ySize)
        {
            X_Size = xSize;
            Y_Size = ySize;
            Fields = new List<Field>();

            for(int i = 0; i < ySize; i++)
            {
                for(int j = 0; j < xSize; j++)
                {
                    Fields.Add(new Field()
                    {
                        X_Position = j + 1,
                        Y_Position = i + 1,
                        Status = 0,
                        ShipId = null
                    });
                }
            }
        }
    }
}
