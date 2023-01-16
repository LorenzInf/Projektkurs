using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour{

	public GameObject room;
    public GameObject player;
    private GameObject current;

	void Awake(){
		CreateLevel(5,5,5);
	}

	public void CreateLevel(int height,int width,int amountOfRooms){
		var rooms=MapGen.Gen(height,width,amountOfRooms);
		int x=0,y=0;
		while(rooms[x,y]==null){
			x++;
			if(x>=height){
				x=0;
				y++;
			}
		}
		current=CreateRoom(x,y,rooms);
	}

	public GameObject CreateRoom(int x,int y,MapGen.Room?[,] r){
		string s=r[x,y].ToString();
		GameObject go=Instantiate(room, new Vector3(x, y, 0), Quaternion.identity);
		RoomController rc=current.GetComponent("RoomController") as RoomController;
		if(rc!=null){
			
		}
		return go;
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
}
