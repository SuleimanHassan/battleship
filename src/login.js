var gameID;

var url = "https://battleship-cmps297r.azurewebsites.net";
var initGameKey = "cqpg4YFUiXjAK3ZKFxxhswTn4c9gKpDwzgwqjlwpmuvJjk952kSMjg==";

$(() => {

})



$("#createBtn").click(() => {
  $.get({
    method: "GET",
    url: `${url}/api/initGame?code=${initGameKey}`,
    success: (data) => {
      gameID = data.gameID;
      document.cookie = `{"gameID": ${gameID}, "playerID": 0}`;   
      window.location = "game.html";
    }
  }) 
})

$("#joinBtn").click(() => {
    gameID = $("#joinIDText").val();
    document.cookie = `{"gameID": ${gameID}, "playerID": 1}`; 
    window.location = "game.html";
})


