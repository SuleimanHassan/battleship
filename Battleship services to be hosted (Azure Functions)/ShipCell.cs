namespace BattleShip {
    public class ShipCell : Cell {
      private Ship ship;
      

      public ShipCell(Ship ship) {
        this.ship = ship;
        isWater = false;
      }

      public override bool IsHit() {
        return isHit;
      }
      
      public override Result shootAt() {
        isHit = true;
        ship.hit();
        return ship.getstate();
      }
    }
}