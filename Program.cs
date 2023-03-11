using Microsoft.Azure.CognitiveServices.Personalizer;
using Microsoft.Azure.CognitiveServices.Personalizer.Models;

namespace SnakeGamePersonalizer
{
    class Programm
    {
        private static readonly string ApiKey = "20814a5e180f4087afa64030bc8c51a6";
        private static readonly string ApiEndpoint = "https://snakegamepersonalizer.cognitiveservices.azure.com/";

        static void Main(string[] args)
        {
            //initiate personaliezr client
            PersonalizerClient client = new PersonalizerClient(new ApiKeyServiceClientCredentials(ApiKey));
            client.Endpoint = ApiEndpoint;

            //initiate values
            string eventId = null;
            List<object> gameState = new List<object>();

            //get rankable actions aka moves
            List<RankableAction> actions = new List<RankableAction>()
            {
                new RankableAction( Direction.Up.ToString(), new List<object> () {Direction.Up }),
                new RankableAction( Direction.Down.ToString(), new List<object> () {Direction.Down }),
                new RankableAction( Direction.Left.ToString(), new List<object> () {Direction.Left }),
                new RankableAction( Direction.Right.ToString(), new List<object> () {Direction.Right }),
            };

            //start game --> everything after this part can be put in for loop or Parallel.Foreach to make this repeatable
            var game = new SnakeGame();
            game.StartGame();

            Thread.Sleep(1000);

            do
            {
                //get current gameState (apple, snake, head) and get eventId for this state
                gameState = game.RetrieveGameState();
                eventId = Guid.NewGuid().ToString();

                //create the request and send it
                RankRequest request = new RankRequest(actions, gameState, null, eventId);
                RankResponse response = client.Rank(request);

                //parse the response and move according to personalizer
                int personalizerMove = int.Parse(response.RewardActionId);
                game.MoveSnake((Direction) personalizerMove);

                //rank the personalizer move
                double reward = game.GetReward();
                client.Reward(eventId, reward);

                Thread.Sleep(1000);
            }
            while (game.GetIsRunning());
        }
    }
}

