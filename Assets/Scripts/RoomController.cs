using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
    
    public LevelController c;
    private MapGen.RoomType Type;
    private GameObject left=null, right=null, up=null, down=null;

    public void Go(MapGen.Dir dir) {
        if (dir == MapGen.Dir.Left && left != null)
            c.Move(left);
        else if (dir == MapGen.Dir.Right && right != null)
            c.Move(right);
        else if (dir == MapGen.Dir.Up && up != null)
            c.Move(up);
        else if (dir == MapGen.Dir.Down && down != null)
            c.Move(down);
    }
    
}
