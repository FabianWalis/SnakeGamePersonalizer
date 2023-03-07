using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace SnakeGamePersonalizer
{
    public class SnakeGame
    {
        public List<Position> snake;
        public Position head;
        public Position apple;
        public bool isRunning;

        public void StartGame()
        {
            snake = new List<Position>() {
                new Position(6,6),
                new Position(6,7),
                new Position(6,8),
            };

            head = snake.First();
            GenerateApple();

            isRunning = true;

            PrintGameState();
        }

        /// <summary>
        /// generates a valid apple
        /// </summary>
        public void GenerateApple()
        {
            var random = new Random();
            do
            {
                apple = new Position(random.Next(11), random.Next(11));
            }
            while (!AppleIsValid());
        }

        /// <summary>
        /// Verifys valid position of the apple
        /// </summary>
        /// <returns>Apple is correct</returns>
        public bool AppleIsValid()
        {
            if (apple.x < 0 && apple.y < 0 && apple.x > 11 && apple.y > 11) //out of bounds
                return false;

            foreach (var position in snake) //apple is in snake
            {
                if (apple.Equals(position)) return false;
            }
            return true;
        }

        public void PrintGameState()
        {
            Console.Clear();

            for (int i = 1; i < 12; i++)
            {
                Console.SetCursorPosition(13, i);
                Console.WriteLine("|");
                Console.SetCursorPosition(0, i);
                Console.WriteLine("|");
            }
            for (int i = 0; i < 13; i++)
            {
                Console.SetCursorPosition(i, 12);
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
    }
}
