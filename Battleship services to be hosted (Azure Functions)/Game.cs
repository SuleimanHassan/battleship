using System;
using System.Collections.Generic;

namespace BattleShip {
  public class Game {
    public Player[] players;
    public Player winner;

    public Game() {
      players = new Player[]{
        new Player(),
        new Player()
      };
      
      players[0].IsPlayerTurn = true;
      players[1].IsPlayerTurn = false;

      winner = null;
    }

    public void nextTurn() {
      players[0].IsPlayerTurn = !players[0].IsPlayerTurn;
      players[1].IsPlayerTurn = !players[1].IsPlayerTurn;

      if(players[0].TotalLives == 0) {
        winner = players[0];
      } else if(players[1].TotalLives == 0) {
        winner = players[0];
      }
    }


  }
}