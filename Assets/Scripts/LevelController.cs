using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour{

    public GameObject player;
    private GameObject current;

	public void start(){
		var mg=new MapGen(this);
		mg.GenerateMap(100,100,5);
	}

    public void MovePlayer(GameObject room){
        current = room;
        player.transform.position = room.transform.position;
    }

    public void Move(MapGen.Dir dir){
		RoomController rc=current.GetComponent("RoomController") as RoomController;
		if(rc!=null)
			rc.Move(dir);
    }

	public void SetCurrentRoom(GameObject room){
		current=room;
	}
}
