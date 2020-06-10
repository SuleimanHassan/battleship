using System;

namespace BattleShip {
  public class Player {
    private string name;
    public string Name {
        get { return name;}
        set { name = value;}
    }
    
    private int totalLives = 17;
    public int TotalLives {
        get { return totalLives;}
        set { totalLives = value;}
    }

    private Board board;
    public Board Board {
        get { return board;}
    }
    
    private bool isplayerTurn;
    public bool IsPlayerTurn
    {
        get { return isplayerTurn;}
        set { isplayerTurn = value;}
    }
    

    public Player() {
      this.board = new Board();
    }


    public void shootAt(Player enemy, int x, int y) {
      if(x >= 0 && x <= 9 && y >= 0 && y <= 9) {
        Cell cell = enemy.Board.getCell(x, y);
        
        if(cell.IsHit()) {
          throw new Exception();
        } else {
          Result result = cell.shootAt();
          if (result == Result.PARTIAL_HIT || result == Result.DESTROYED) {
            enemy.totalLives--;
          }  
        }
      } else {
        throw new Exception();
      }        
    }
  }
}