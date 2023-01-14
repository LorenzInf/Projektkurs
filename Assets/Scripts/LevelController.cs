using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour{
    public GameObject player;
    private GameObject current;

    public void Move(GameObject room){
        current = room;
        player.transform.position = room.transform.position;
    }

    public void Move(MapGen.Dir dir){
        if (current.GetComponent("RoomController") != null) {
            //current.GetComponent("RoomController").Go(dir); TODO fix
        }
    }
}
