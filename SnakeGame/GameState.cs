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

        public GameState (Position apple, List<Position> snake, Position head)
        {
            this.apple = apple.ToString();
            this.snake = printSnake(snake);
            this.head = head.ToString();
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
