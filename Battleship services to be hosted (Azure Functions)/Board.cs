using System;
using System.Collections.Generic;

namespace BattleShip {
  public class Board {
    public int size = 10;
    public Cell[ , ] board;
    public Ship[] ships;

    public Board() {
      ships = new Ship[] {
        new Ship("Carrier", 5),
        new Ship("Battleship", 4),
        new Ship("Cruiser", 3),
        new Ship("Submarine", 3),
        new Ship("Destroyer", 2)
      };

      board = new Cell[size, size];
      for (int i = 0; i < size; i++) {
          for (int j = 0; j < size; j++) {
            board[i, j] = new WaterCell();
          }
      } 
    }

    public Cell getCell(int x, int y) {
      if (isInBoard(x, y)) {
        return board[x, y];
      }
      throw new Exception();
    }

    public void placeShips(List<reqSetShip> s) {

      if(s.Count != 5) {
        throw new Exception();
      }
        
      for (int i = 0; i < 5; i++)
      {
        if(!isValidPlacement(s[i].x, s[i].y, ships[i].Size, s[i].isHorizontal)) {
          throw new Exception();
        } else {
          placeShip(ships[i], s[i].x, s[i].y, s[i].isHorizontal);
        }
      }
    }


    
    //Helpers
    private bool isValidPlacement(int x, int y, int Shipsize, bool horizontal) {
      int xDel = 0;
      int yDel = 0;
      if (horizontal) {
        xDel = 1;
      } else {
        yDel = 1;
      }

      if(!isInBoard(x, y) || ((!isInBoard(x + Shipsize, y) && horizontal)) || ((!isInBoard(x, y + Shipsize) && !horizontal))) {
        return false;
      }
      
      for (int i = 0; i < Shipsize; i++) {
        if (board[x + i * xDel, y + i * yDel].GetType()  != typeof(WaterCell)) {
            return false;
        }
      }
      return true;
    }

    private void placeShip(Ship ship, int x, int y, bool horizontal) {
      int xDel = 0;
      int yDel = 0;
      if (horizontal) {
        xDel = 1;
      } else {
        yDel = 1;
      }

      for (int i = 0; i < ship.Size; i++) {
        board[x + i * xDel, y + i * yDel] =  new ShipCell(ship);
      }
    }

    private bool isInBoard(int x, int y) {
      return (x >= 0 && x < size && y >= 0 && y < size);
    }
  }

}