using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject player;
    public GameObject wall;
    public GameObject floor;

    public void Start(){
        CreateLevel();
    }

    public void ReadInput(string input){
        
    }

    public void CreateLevel()
    {

    }

    private void CreateObject(string typ,float x,float y) {
        GameObject go=null;
        switch (typ){
            case "wall":
                go = Instantiate(wall);
                break;
            case "floor":
                go = Instantiate(floor);
                break;
        }

        if (go != null)
            go.transform.position += new Vector3(x, y, 0f);
    }
}
