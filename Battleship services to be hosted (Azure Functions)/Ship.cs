using System;

namespace BattleShip {
    public class Ship {
      private string name;
      public string Name {
        get { return name; }
      }

      private int size;
      public int Size {
        get { return size; }
      }

      private int lives;
      public int Lives {
        get { return lives; }
      }



      public Ship(string name, int size) {
        this.name = name;
        this.size = size;
        this.lives = size;
      }



      public void hit() {
        if(lives > 0) {
          System.Console.WriteLine($"{name} was hit!");
          lives--;
        } else {
          System.Console.WriteLine($"{name} was destroyed!");
        }
      }

      public Result getstate() {
        if (lives == 0) {
            return Result.DESTROYED;
        } else if (lives < size) {
            return Result.PARTIAL_HIT;
        } else {
            return Result.NO_HIT;
        }
      }
      

    }
}