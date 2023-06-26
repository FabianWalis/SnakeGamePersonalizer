using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.InteropServices;

namespace SnakeGamePersonalizer
{
    public class SnakeGame
    {
        private List<Position> snake;
        private Position head;
        private Position apple;
        private bool isRunning;

        private double reward;

        public void StartGame()
        {
            snake = new List<Position>() {
                new Position(6,6),
                new Position(6,7),
                new Position(6,8)
            };

            head = snake.First();
            GenerateApple();

            isRunning = true;

            PrintGameState();
        }

        /// <summary>
        /// moves the snake in the wanted direction
        /// </summary>
        /// <param name="direction"></param>
        public void MoveSnake(Direction direction)
        {
            reward = CalculateMoveReward();

            Position tempHead = new Position(head.x, head.y);

            switch (direction)
            {
                case Direction.Left:
                    tempHead.x--;
                    break;
                case Direction.Right:
                    tempHead.x++;
                    break;
                case Direction.Up:
                    tempHead.y--;
                    break;
                case Direction.Down:
                    tempHead.y++;
                    break;
                default:
                    isRunning = false;
                    return;
            }

            if (!IsInbounds(tempHead))
            {
                isRunning = false;
                return;
            }

            //apple is not hit so we need to remove last element (as we previously added a new head)
            if (!tempHead.Equals(apple))
                snake.RemoveAt(snake.Count - 1);
            else //apple is hit, we don't remove and calc new apple
                GenerateApple();

            //check if snake is hit
            if (HitSnake(tempHead))
            {
                isRunning = false;
                return;
            }

            //new head is inBounds and does not hit snake so we set it as new head
            snake.Insert(0, tempHead);
            head = snake.First();

            PrintGameState();
        }

        /// <summary>
        /// generates a valid apple
        /// </summary>
        private void GenerateApple()
        {
            var random = new Random();
            do
            {
                apple = new Position(random.Next(11), random.Next(11));
            }
            while (!AppleIsValid());
        }

        public IList<object> RetrieveGameState()
        {
            IList<object> gameState = new List<object>()
            {
                new GameState(apple, snake, head)

            };

            return gameState;
        }

        private double CalculateMoveReward()
        {
            //TODO
            return 0.5;
        }


        /// <summary>
        /// Verifys valid position of the apple
        /// </summary>
        /// <returns>Apple is correct</returns>
        private bool AppleIsValid()
        {
            if (!IsInbounds(apple))
                return false;

            if (HitSnake(apple))
                return false;

            return true;
        }

        private bool IsInbounds(Position position)
        {
            if (position.x < 0 || position.y < 0 || position.x > 11 || position.y > 11) //out of bounds
                return false;
            return true;
        }

        private bool HitSnake(Position position)
        {
            foreach (var body in snake) //position is in snake
            {
                if (body.Equals(position)) return true;
            }
            return false;
        }

        public void PrintGameState()
        {
            Console.Clear();

            for (int i = 1; i < 13; i++)
            {
                Console.SetCursorPosition(13, i);
                Console.WriteLine("|");
                Console.SetCursorPosition(0, i);
                Console.WriteLine("|");
            }
            for (int i = 0; i < 14; i++)
            {
                Console.SetCursorPosition(i, 13);
                Console.WriteLine("¯");
                Console.SetCursorPosition(i, 0);
                Console.WriteLine("_");
            }

            Console.SetCursorPosition(apple.x + 1, apple.y + 1);
            Console.Write("@");

            foreach (var position in snake)
            {
                Console.SetCursorPosition(position.x + 1, position.y + 1);
                Console.Write("o");
            }

            Console.SetCursorPosition(0, 14);
        }

        public double GetReward()
        {
            return reward;
        }

        public bool GetIsRunning()
        {
            return isRunning;
        }   
    }
}
