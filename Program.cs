using Microsoft.Azure.CognitiveServices.Personalizer;
using Microsoft.Azure.CognitiveServices.Personalizer.Models;
using System.Security.Cryptography.X509Certificates;

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
            string eventId = string.Empty;
            IList<object> gameState = new List<object>();

            //get rankable actions aka moves
            IList<RankableAction> actions = new List<RankableAction>()
            {
                new RankableAction( id: "0", features: new List<object> () { new { direction = (int) Direction.Up }}),
                new RankableAction( id: "1", features: new List<object> () { new { direction = (int) Direction.Down } }),
                new RankableAction( id: "2", features: new List<object> () { new { direction = (int) Direction.Left } }),
                new RankableAction( id: "3", features: new List<object> () { new { direction = (int) Direction.Right }}),
            };

            int iterations = 1;
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
                    var aiMove = (Direction)personalizerMove;

                    //rank the personalizer move
                    double reward = GetReward(game, aiMove);
                    client.Reward(eventId, reward);

                    //perform move
                    game.MoveSnake(aiMove);

                    //sleep for better visualization
                    Thread.Sleep(1000);
                }
                while (game.GetIsRunning());
            }
        }

        static double GetReward(SnakeGame currentGame, Direction direction)
        {
            //we compare both of the gamestate before and after moving
            SnakeGame movedGame = currentGame;
            movedGame.MoveSnake(direction);

            SnakeGame unmodifiedGame = currentGame;

            if (!movedGame.GetIsRunning()) //case we are dead
                return 0;
            else if (movedGame.GetSnake().Count() > unmodifiedGame.GetSnake().Count()) //case we are alive and got the apple
                return 1;
            else //case we are alive and did not get the apple
            {  
                int movesUntilAppleMovedGame = GetFastestSecureWayToApple(movedGame);
                int movesUntilAppleUnmodifiedGame = GetFastestSecureWayToApple(unmodifiedGame);

                //cases for when we can not or could not hit the apple anymore
                if (movesUntilAppleMovedGame == -1 && movesUntilAppleUnmodifiedGame > 0) //case we blocked ourselves + no way to next apple
                    return 0.05;
                else if (movesUntilAppleMovedGame == -1 && movesUntilAppleUnmodifiedGame == -1) //case were blocked and cannot get apple --> due to bad previous choices
                    return 0.25;
                else if (movesUntilAppleMovedGame > 0 && movesUntilAppleUnmodifiedGame == -1) // case we uncornered ourselves (should technically not be a possible option)
                    return 1;
                else if (movesUntilAppleMovedGame > 0 && movesUntilAppleUnmodifiedGame > 0) //cases where we can hit the apple
                {
                    bool movedGameCornered = IsGameCornered(movedGame);
                    bool unmodifiedGameCornered = IsGameCornered(unmodifiedGame);
                    int moveDifference = movesUntilAppleUnmodifiedGame - movesUntilAppleMovedGame;

                    //cases where we are cornered with the apple
                    if (movedGameCornered && !unmodifiedGameCornered) //case we cornered ourselves
                    { 
                        if (moveDifference > 0) // case we cornered and strayed from the fastest path but did not corner us
                            return 0.05;
                        else if (moveDifference == 0) // case we are cornered but should not happen as a move either brings us further towards or from the apple but on case
                            return 0.1;
                        else if (moveDifference < 0) // case were we are cornered and took a safe step towards the apple
                            return 0.15;
                    }
                    else // case we were already cornered, are no longer cornered (should not happen) or are free --> are treated as same as no critical flaw was made by the AI as we can still get to the apple and did not corner us in this move
                    {
                        if (moveDifference > 0) // case we strayed from the fastest path but did not corner us
                            return 0.2;
                        else if (moveDifference == 0) // case should not happen as a move either brings us further towards or from the apple but on case
                            return 0.5;
                        else if (moveDifference < 0) // case were we took a safe step towards the apple
                            return 1;
                    }
                }
            }
            return 0.25; // default if i missed a case
        }

        static int GetFastestSecureWayToApple(SnakeGame game)
        {
            //TODO
            return 0;
        }

        static bool IsGameCornered(SnakeGame game)
            {
                return false;
            }
    }
}

