using System.Collections.Generic;

namespace BattleShip
{
    public class reqGame {
      public int gameId { get;set; }
    }

    
    public class reqSetShip {
      public bool isHorizontal { get;set; }
      public int x { get;set; }
      public int y { get;set; }
    }


    public class reqSetPlayer : reqGame {
      public string playerName { get;set; }
      public int playerNum { get;set; }

      public List<reqSetShip> ships { get;set; }
    }

    public class reqSetShootCoords : reqGame {
      public int playerNum { get;set; }

      public int x { get;set; }
      public int y { get;set; }     
    }
}