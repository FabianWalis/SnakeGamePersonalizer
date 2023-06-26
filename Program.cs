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
            IList<object> gameState = new List<object>();

            //get rankable actions aka moves
            IList<RankableAction> actions = new List<RankableAction>()
            {
                new RankableAction( id: "0", features: new List<object> () { new { direction = (int) Direction.Up }}),
                new RankableAction( id: "1", features: new List<object> () { new { direction = (int) Direction.Down } }),
                new RankableAction( id: "2", features: new List<object> () { new { direction = (int) Direction.Left } }),
                new RankableAction( id: "3", features: new List<object> () { new { direction = (int) Direction.Right }}),
            };

            int iterations = 10;
            //this can be in a parallel foreach but data has to be locked and stuff
            for (int i = 0; i < iterations; i++)
            {
                var game = new SnakeGame();
                game.StartGame();

                Thread.Sleep(1000);

                do
                {
                    //get current gameState (apple, snake, head) and get eventId for this state
                    gameState = game.RetrieveGameState();
                    eventId = Guid.NewGuid().ToString();

                    //create the request and send it
                    RankRequest request = new RankRequest(actions: actions, contextFeatures: gameState, eventId: eventId);
                    eventId = request.EventId;

                    RankResponse response = client.Rank(request);

                    //parse the response and move according to personalizer
                    int personalizerMove = int.Parse(response.RewardActionId);
                    game.MoveSnake((Direction)personalizerMove);

                    //rank the personalizer move
                    double reward = game.GetReward();
                    client.Reward(eventId, reward);

                    Thread.Sleep(1000);
                }
                while (game.GetIsRunning());
            }
        }
    }
}

