using Microsoft.Azure.CognitiveServices.Personalizer;
using Microsoft.Azure.CognitiveServices.Personalizer.Models;

using System;
using System.Linq;

namespace SnakeGamePersonalizer
{
    class Programm
    {
        private static readonly string ApiKey = "20814a5e180f4087afa64030bc8c51a6";
        private static readonly string ApiEndpoint = "https://snakegamepersonalizer.cognitiveservices.azure.com/";

        static void Main(string[] args)
        {
            SnakeGame sg = new SnakeGame();
            sg.StartGame();
            //PersonalizerPlayGame();
        }

        static void PersonalizerPlayGame() { 
            int iteration = 1;
            bool runLoop = true;

            IList<RankableAction> actions = GetActions();
            PersonalizerClient client = InitializePersonalizerClient();


            /* Explanation: Azure Personalizer is ranking rewards towards a certain context --> it makes an action depending on the given context
             * Plan: In order to make it play the game and decide where to go next, a current gamestate is presented and Personalizer has to choose the next move, sadly the time factor of the snake game can not be factored in
             */
            do
            {
                
            } while (runLoop);
        }

        /// <summary>
        /// creates a new Personalizer Client with the globally specified Key and Endpoint
        /// </summary>
        static PersonalizerClient InitializePersonalizerClient()
        {
            PersonalizerClient client = new PersonalizerClient( new ApiKeyServiceClientCredentials(ApiKey));
            client.Endpoint = ApiEndpoint;

            return client;
        }

        /// <summary>
        /// Returns a list of actions to rank
        /// </summary>
        static IList<RankableAction> GetActions()
        {
            IList<RankableAction> actions = new List<RankableAction>();
            return actions;
        }
    }
}

