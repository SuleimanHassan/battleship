var gameID;
var playerID;
var enemyID;

var url = "https://battleship-cmps297r.azurewebsites.net";
var gameStateKey = "HafjytvyaOEPHGUSrzxtvNMHXNawCxsdqAnkAx/mJAcAMFh9kFluoQ==";
var setPlayerKey = "F1IqbJm8mc5mVlrOCvenkZ8D1bItCAeZrsWtsSTNnKvnx7gt8A0W0A==";
var turnPlayerKey = "QMIliHPxKTHQxaZ0daqN9bJ/y6TDhsTH9UokBlIj4sPOFTpfTWRvAQ==";

var gameState = null;

var letters = Array("", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J");
var cellSize = 30;
var cell;
var selected = null;
var selectedX;
var selectedY;

async function updateGame() {
  await $.post({
    method: "POST",
    url: `${url}/api/gameState?code=${gameStateKey}`,
    data: JSON.stringify({gameID: gameID}),
    contentType: "application/json",
    dataType: "json",
    success: (data) => {
      gameState = data;
    }
  })
  
  console.log(gameState);
  if(gameState.players[playerID].Name != null) {
    $("#playerName").html(gameState.players[playerID].Name);
  }
  if(gameState.players[enemyID].Name != null) {
    $("#enemyName").html(gameState.players[enemyID].Name);
  }

  createPlayerBoard(gameState.players[playerID].Board);
  createEnemyBoard(gameState.players[enemyID].Board);
}


function createEnemyBoard(Board) {
  boardDiv = $("#enemyBoard");
  boardDiv.html("");

  for(var i = 0; i <= 10; i++) {
    cell = $(document.createElement("div"));
    cell
      .addClass("cell")
      .html(letters[i])
      .width(cellSize).height(cellSize)
      .appendTo("#enemyBoard");
  }
  $(document.createElement("br")).appendTo("#enemyBoard");

  for(var i = 0; i < 10; i++) {
    cell = $(document.createElement("div"));
    cell
      .addClass("cell")
      .html(i + 1)
      .width(cellSize).height(cellSize)
      .appendTo("#enemyBoard");

    for(var j = 0; j < 10; j++) {
      coord = i + 1 + ", " + letters[j];

      cell = $(document.createElement("div"));
      cell
        .addClass("cell")
        .attr("alt", coord)
        .width(cellSize).height(cellSize)
        .data("row", i).data("col", j)
        .on("click", cellClick)
        .appendTo("#enemyBoard");

      var boardCell = Board.board[j][i];
      if(boardCell.isWater && boardCell.isHit) {
        cell.addClass("waterHit");
      } else if(boardCell.isWater){
        cell.addClass("water");
      }else if(!boardCell.isWater && boardCell.isHit){
        cell.addClass("shipHit");
      } else {
        cell.addClass("water");
      }

      if(j == selectedX && i == selectedY) {
        selected = cell;
        selected.addClass("selected");
        selectedX = selected.data("col");
        selectedY = selected.data("row");
      }
    }   
    $(document.createElement("br")).appendTo("#enemyBoard");

  }
}
function createPlayerBoard(Board) {
  boardDiv = $("#playerBoard");
  boardDiv.html("");

  for(var i = 0; i <= 10; i++) {
    cell = $(document.createElement("div"));
    cell
      .addClass("cell")
      .html(letters[i])
      .width(cellSize).height(cellSize)
      .appendTo("#playerBoard");
  }
  $(document.createElement("br")).appendTo("#playerBoard");

  for(var i = 0; i < 10; i++) {
    cell = $(document.createElement("div"));
    cell
      .addClass("cell")
      .html(i + 1)
      .width(cellSize).height(cellSize)
      .appendTo("#playerBoard");

    for(var j = 0; j < 10; j++) {
      coord = i + 1 + ", " + letters[j];

      cell = $(document.createElement("div"));
      cell
        .addClass("cell")
        .attr("alt", coord)
        .width(cellSize).height(cellSize)
        .data("row", i).data("col", j) 
        .appendTo("#playerBoard");

      var boardCell = Board.board[j][i];
      if(boardCell.isWater && boardCell.isHit) {
        cell.addClass("waterHit");
      } else if(boardCell.isWater){
        cell.addClass("water");
      }else if(!boardCell.isWater && boardCell.isHit){
        cell.addClass("shipHit");
      } else {
        cell.addClass("ship");
      }
    }   
    $(document.createElement("br")).appendTo("#playerBoard");

  }
}



function cellClick() {
  if(selected != null) {
    selected.removeClass("selected");
  } 
  selected = $(this)
  selected.addClass("selected");
  selectedX = selected.data("col");
  selectedY = selected.data("row");
}



$(() => {
  var cookieData = JSON.parse((document.cookie));

  gameID = cookieData.gameID;
  playerID = cookieData.playerID;
  enemyID = (playerID + 1) % 2;

  $("#footerLeft").html(`Game ID: ${gameID}   | Player ID: ${playerID}`);

  updateGame();

  console.log(gameState);
  setInterval(() => {
    updateGame();

    if(gameState.winner != null){
      $("#footerRight").html(`Game Status: Game Complete`);   
    }else if(gameState.players[playerID].Name == null) {
      $("#footerRight").html(`Game Status: Setting your board`);
    } else if(gameState.players[enemyID].Name == null) {
      $("#footerRight").html(`Game Status: Enemy is setting their board`);
    } else if(gameState.players[playerID].IsPlayerTurn) {
      $("#footerRight").html(`Game Status: Your Turn`);
    } else {
      $("#footerRight").html(`Game Status: Enemy Turn`);
    }
  }, 5000)
});



//Set Player
$("#setSubmit").click((e) => {
  e.preventDefault();
  var json = {
  PlayerName: $("#setName").val(),
  PlayerNum: playerID,
  ships: Array(
    { isHorizontal: $("#s1h").is(':checked'), x: $("#s1x").val() -1, y: $("#s1y").val() -1},
    { isHorizontal: $("#s2h").is(':checked'), x: $("#s2x").val() -1, y: $("#s2y").val() -1},
    { isHorizontal: $("#s3h").is(':checked'), x: $("#s3x").val() -1, y: $("#s3y").val() -1},
    { isHorizontal: $("#s4h").is(':checked'), x: $("#s4x").val() -1, y: $("#s4y").val() -1},
    { isHorizontal: $("#s5h").is(':checked'), x: $("#s5x").val() -1, y: $("#s5y").val() -1}
  ),
  gameID: gameID
  }
  console.log(json);

  $.post({
    method: "POST",
    url: `${url}/api/setPlayer?code=${setPlayerKey}`,
    data: JSON.stringify(json),
    contentType: "application/json",
    dataType: "json",
    success: (data) => {
      updateGame();
      $("#setForm").hide();
    }
  })
});



//Shoot
$("#shoot").click((e) => {
  e.preventDefault();
  if(selected != null) {
    var json = {
      PlayerNum: playerID,
      x: selectedX,
      y: selectedY,
      gameID: gameID
    }
    console.log(json);
  
    $.post({
      method: "POST",
      url: `${url}/api/shootAt?code=${turnPlayerKey}`,
      data: JSON.stringify(json),
      contentType: "application/json",
      dataType: "json",
      success: (data) => {
        selected.removeClass("selected");
        selectedX = -1;
        selectedY = -1;
        updateGame();
      }
    })
  }  
})


function winnerCheck() {
  if(gameState.winner != null) {
    $("#game").hide();
    $("<h1></h1>").html(`Winner is: ${gameState.winner.Name}`).appendTo("$game");
  }

}