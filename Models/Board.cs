using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Board
    {
        private enum FieldStatus
        {
            Empty = 1,
            Filled = 2,
            Hit = 3,
            HitAndSink = 4
        }

        private FieldStatus[,] board;

        Board()
        {
            board = new FieldStatus[10, 10];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = 0;
                    Console.WriteLine("Element({0},{1}) = {2}", i, j, board[i, j]);
                }
            }
        }
    }
}
