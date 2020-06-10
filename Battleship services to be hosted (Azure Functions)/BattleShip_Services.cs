using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;

namespace BattleShip
{
    public static class BattleShip_Services
    {   
        private static Program program = new Program();



        [FunctionName("initGame")]
        public static async Task<HttpResponseMessage> initGame(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "initGame")] HttpRequestMessage  req,
            ILogger log)
        {

          int gameID = program.createNewGame();


          var response = new HttpResponseMessage(HttpStatusCode.OK);
          response.Content = new StringContent($"{{ \"gameID\": {gameID} }}");
          response.Headers.Add("Access-Control-Allow-Credentials", "true");
          response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

          return response;          
        }




        [FunctionName("setPlayer")]
        public static async Task<HttpResponseMessage> setPlayer(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "setPlayer")] HttpRequestMessage  req,
            ILogger log)
        {
          var data = await req.Content.ReadAsAsync<reqSetPlayer>();
            
          if(data.playerNum > 1) {
            return badResponse("Player does not exist");
          }
          Game game = program.getGameInstance(data.gameId);
          var player = game.players[data.playerNum];
          player.Name = data.playerName;


          try {
            player.Board.placeShips(data.ships);
          } catch(Exception) {
            return badResponse("Invalid Ship Placement");
          }

          var response = new HttpResponseMessage(HttpStatusCode.OK);
          response.Content = new StringContent(JsonConvert.SerializeObject(player, Formatting.Indented));
          response.Headers.Add("Access-Control-Allow-Credentials", "true");
          response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
          return response;          
        }




        [FunctionName("shootAt")]
        public static async Task<HttpResponseMessage> shootAt(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "shootAt")] HttpRequestMessage  req,
            ILogger log)
        {
          var data = await req.Content.ReadAsAsync<reqSetShootCoords>();
          
          if(data.playerNum > 1) {
            return badResponse("Player does not exist");
          }

          Game game = program.getGameInstance(data.gameId);
          var player = game.players[data.playerNum];
          var enemy = game.players[(data.playerNum + 1) % 2];

          if(!player.IsPlayerTurn) {
            return badResponse("Not your turn!");
          } 


          try {
            player.shootAt(enemy, data.x, data.y);
          } catch(Exception) {
            return badResponse("Invalid Shooting");
          }

          game.nextTurn();

          var response = new HttpResponseMessage(HttpStatusCode.OK);
          response.Content = new StringContent(JsonConvert.SerializeObject(game, Formatting.Indented));
          response.Headers.Add("Access-Control-Allow-Credentials", "true");
          response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
          return response;          
        }


        [FunctionName("gameState")]
        public static async Task<HttpResponseMessage> gameState(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "gameState")] HttpRequestMessage  req,
            ILogger log)
        {
          var data = await req.Content.ReadAsAsync<reqGame>();
          Game game = program.getGameInstance(data.gameId);   

          var response = new HttpResponseMessage(HttpStatusCode.OK);
          response.Content = new StringContent(JsonConvert.SerializeObject(game, Formatting.Indented));
          response.Headers.Add("Access-Control-Allow-Credentials", "true");
          response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
          return response;          
        }



        private static HttpResponseMessage badResponse(string msg) {
          var response = new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
          response.Content = new StringContent(msg);
          return response;
        }
    }
}
