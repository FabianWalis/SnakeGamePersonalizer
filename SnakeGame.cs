using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePersonalizer
{
    internal class SnakeGame
    {

        public Position _apple;
        public Snake _snake;
        /// <summary>
        /// starts the game
        /// </summary>
        public void StartGame()
        {
            _snake = new Snake();
            _apple = CreateApple(_snake);

            while (!_snake.dead)
            {
                Render(_snake.body, _apple);
                Move(_snake, _apple);
            }
            if (_snake.dead)
            {
                Console.Clear();
                Console.WriteLine("You Lost");
            }
        }

        private void Move(Snake snake, Position apple)
        {
            Console.WriteLine("Where to move? (W = up, A = left, S = down, D = right)");

            //User has to select W,A,S,D. If not W,A,S,D it repeats until it gets one
            var inputDirection = Console.ReadKey().Key;
            while (inputDirection != ConsoleKey.W && inputDirection != ConsoleKey.A && inputDirection != ConsoleKey.S && inputDirection != ConsoleKey.D)
            {
                inputDirection = Console.ReadKey().Key;
            }

            switch (inputDirection)
            {
                case ConsoleKey.W:
                    {
                        snake.Move(Direction.Up); break;
                    }
                case ConsoleKey.A:
                    {
                        snake.Move(Direction.Left); break;
                    }
                case ConsoleKey.S:
                    {
                        snake.Move(Direction.Down); break;
                    };
                case ConsoleKey.D:
                    {
                        snake.Move(Direction.Right); break;
                    }
            }
            if(snake.head.up == apple.up && snake.head.left == apple.left)
            {
                snake.hasEaten = true;
                _apple = CreateApple(snake);
            }
        }

        /// <summary>
        /// Renders the current gameState and shows it in the Console
        /// </summary>
        private void Render(List<Position> snakeBody, Position apple)
        {
            Console.Clear();

            for (int i = 1; i < 12; i++)
            {
                Console.SetCursorPosition(12, i);
                Console.WriteLine("|");
                Console.SetCursorPosition(0, i);
                Console.WriteLine("|");
            }
            for (int i = 0; i < 13; i++)
            {
                Console.SetCursorPosition(i,12);
                Console.WriteLine("¯");
                Console.SetCursorPosition(i,0);
                Console.WriteLine("_");
            }


            Console.SetCursorPosition(apple.left+1, apple.up+1);
            Console.Write("@");

            foreach (var pos in snakeBody)
            {
                Console.SetCursorPosition(pos.left+1, pos.up+1);
                Console.Write("o");
            }

            Console.SetCursorPosition(0, 14);
        }

        private Position CreateApple(Snake snake)
        {
            Position apple = new Position();
            var rand = new Random();
            apple.up = rand.Next(0, 11);
            apple.left = rand.Next(0, 11);

            while (snake.body.Contains(apple))
            {
                rand = new Random();
                apple.up = rand.Next(0, 11);
                apple.left = rand.Next(0, 11);
            }

            return apple;
        }
    }
    public class Snake
    {
        public List<Position> body;
        public Position head;
        public bool dead;
        public bool hasEaten;

        /// <summary>
        /// Snake always starts with body length of 3 and on the same position facing upwards
        /// </summary>
        public Snake()
        {
            body = new List<Position>() { new Position(5, 6), new Position(6, 6), new Position(7, 6) };
            head = body.First();
            dead = false;
            hasEaten = false;
        }

        public void Move(Direction direction)
        {

            Position newHead = null;

            switch (direction)
            {
                case Direction.Left:
                    {
                        newHead = head.moveLeftRight(-1);
                        break;
                    }
                case Direction.Right:
                    {
                        newHead = head.moveLeftRight(1);
                        break;
                    }
                case Direction.Up:
                    {
                        newHead = head.moveUpDown(-1);
                        break;
                    }
                case Direction.Down:
                    {
                        newHead = head.moveUpDown(1);
                        break;
                    }
                default: throw new ArgumentOutOfRangeException();
            }

            if (body.Contains(newHead) || !newHead.ValidatePosition())
            {
                dead = true;
                return;
            }

            body.Insert(0, newHead);
            head = body.First();
            if (hasEaten) hasEaten = false;
            else body.RemoveAt(body.Count - 1);
        }
    }

    public class Position
    {
        public Position(int up, int left)
        {
            this.up = up;
            this.left = left;
        }

        public Position()
        {
            up = 0;
            left = 0;
        }

        /// <summary>
        /// value for the up and down position
        /// </summary>
        public int up { get; set; }

        /// <summary>
        /// value for the left and right position
        /// </summary>
        public int left { get; set; }

        /// <summary>
        /// Moves the Snake Up or Down
        /// </summary>
        /// <param name="n">Number of tiles moved: will only be used as 1 or -1</param>
        /// <returns>The new Position of the Snake</returns>
        public Position moveUpDown(int n)
        {
            return new Position(up + n, left);
        }

        /// <summary>
        /// Moves the Snake Left or Right
        /// </summary>
        /// <param name="n">Number of tiles moved: will only be used as 1 or -1</param>
        /// <returns>The new Position of the Snake</returns>
        public Position moveLeftRight(int n)
        {
            return new Position(up, left + n);
        }

        public bool ValidatePosition()
        {
            if (up < 0 || up > 10) return false;
            if (left < 0 || left > 10) return false;
            return true;
        }
    }

    public enum Direction
    {
        Up, Down, Left, Right
    }
}
