using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePersonalizer
{
    public class GameState
    {
        public string apple { get; set; }
        public string snake { get; set; }
        public string head { get; set; }
        public int snakeLength { get; set; }
        public bool alive { get; set; }


        public GameState (Position apple, List<Position> snake, Position head, int snakeLength, bool alive)
        {
            this.apple = apple.ToString();
            this.snake = printSnake(snake);
            this.head = head.ToString();
            this.snakeLength = snakeLength;
            this.alive = alive;
        }

        public string printSnake(List<Position> snake)
        {
            var snakeBuilder = new StringBuilder();
            foreach (var position in snake)
            {
                snakeBuilder.Append(position.ToString());
            }
            return snakeBuilder.ToString();
        }
    }
}
