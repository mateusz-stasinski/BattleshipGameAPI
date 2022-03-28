using Domain.Entities.Ships;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public bool IsEnded { get; set; }
        public int? WinnerId { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public void StartNewGame(string firstPlayerName, string secondPlayerName)
        {
            IsEnded = false;
            WinnerId = null;


            Player firstPlayer = AddPlayer(firstPlayerName);

            Player secondPlayer = AddPlayer(secondPlayerName);


            Players = new List<Player>()
            {
                firstPlayer,
                secondPlayer
            };

            
        }

        private Player AddPlayer(string name)
        {
            var player = new Player()
            {
                Name = name,
                Score = 0,
                IsWinner = null,
                Board = new Board(),
                Ships = new List<Ship>()
                {
                    new Battleship(),
                    new Carrier(),
                    new Destroyer(),
                    new PatrolBoat(),
                    new Submarine()
                }
            };
            player.Board.InitializeFields(10, 10);

            return player;
        }

        public void PlaceShip(Ship ship, Board board)
        {
            var randomDirection = new Random();
            var randomPosition = new Random();
            int xPosition;
            int yPosition;

            var shipFieldsAmount = 0;

            while (shipFieldsAmount <= ship.Length)
            {
                Direction direction = (Direction)randomDirection.Next(1, 3);

                if (direction == Direction.horizontal)
                {
                    xPosition = randomPosition.Next(1, board.X_Size - ship.Length + 2);
                    yPosition = randomPosition.Next(1, board.Y_Size);

                    for (int i = xPosition; i < xPosition + ship.Length; i++)
                    {
                        var field = board.Fields.Single(f => f.X_Position == i && f.Y_Position == yPosition);

                        if (field.Status != FieldStatus.Empty)
                        {
                            break;
                        }

                        field.ShipId = ship.Id;
                        field.Status = FieldStatus.Filled;
                    }
                }
                else if (direction == Direction.vertical)
                {
                    xPosition = randomPosition.Next(1, board.X_Size);
                    yPosition = randomPosition.Next(1, board.Y_Size - ship.Length + 2);

                    for (int i = yPosition; i < yPosition + ship.Length; i++)
                    {
                        var field = board.Fields.Single(f => f.X_Position == xPosition && f.Y_Position == i);

                        if (field.Status != FieldStatus.Empty)
                        {
                            break;
                        }

                        field.ShipId = ship.Id;
                        field.Status = FieldStatus.Filled;
                    }
                }
                shipFieldsAmount = board.Fields.Where(f => f.ShipId == ship.Id).ToList().Count();
            }

        }

        private enum Direction
        {
            horizontal = 1,
            vertical = 2
        }
    }
}