namespace BattleShip
{
    public abstract class Cell
    {
      public bool isWater;
      public bool isHit = false;
      
      public abstract bool IsHit();
      public abstract Result shootAt();
    } 
}