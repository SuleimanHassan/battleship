using System;
using System.Collections.Generic;

namespace BattleShip {
  class Program {

    private int gameIDCounter = 0;
    private Dictionary<int, Game> gameInstances = new Dictionary<int, Game>();

    public Program() {
    }
    

    public int createNewGame() {
      gameInstances.Add(gameIDCounter, new Game());
      return gameIDCounter++;
    }

    public Game getGameInstance(int id) {
      return gameInstances[id];
    }
  }
}
