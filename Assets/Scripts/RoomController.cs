using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
    
    public LevelController c;
    private MapGen.RoomType Type;
    private GameObject left=null, right=null, up=null, down=null;

    public void Move(MapGen.Dir dir) {
        if (dir == MapGen.Dir.Left && left != null)
            c.MovePlayer(left);
        else if (dir == MapGen.Dir.Right && right != null)
            c.MovePlayer(right);
        else if (dir == MapGen.Dir.Up && up != null)
            c.MovePlayer(up);
        else if (dir == MapGen.Dir.Down && down != null)
            c.MovePlayer(down);
    }

    public void SetLeft(GameObject go){
        left = go;
    }

    public void SetRight(GameObject go){
        right = go;
    }
    
    public void SetUp(GameObject go){
        up = go;
    }

    public void SetDown(GameObject go){
        down = go;
    }
    
}
