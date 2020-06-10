namespace BattleShip
{
    public class WaterCell : Cell {

        public WaterCell() {
          isWater = true;
        }
        
        public override bool IsHit() {
          return isHit;
        }

        public override Result shootAt() {
          isHit = true;
          return Result.NO_HIT;
        }

    }
}