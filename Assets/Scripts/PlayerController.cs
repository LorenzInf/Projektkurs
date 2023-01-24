using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    
    private static ArrayList _items=new ArrayList();
    private static ItemController _inHand=null;
    private static double _maxHealth=100;
    private static double _health=100;
    private static double _level=1;
    
	private bool left=false;

    public GameObject player;
    public LevelController level=null;
	public bool canMove;

    public void Start(){
        _items = new ArrayList();
        
    }

    public void Update(){
		if(canMove)
        	HandleMovement();
    }

    public void HandleMovement(){
		float x=0.0f,y=0.0f;
        if (Input.GetKey(KeyCode.S))
            y-=2;
        if (Input.GetKey(KeyCode.A))
        	x-=2;
        if (Input.GetKey(KeyCode.W))
            y+=2;
        if (Input.GetKey(KeyCode.D))
            x+=2;
		if(left==x>0&&x!=0.0f){
			gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
			left=!left;
		}
		gameObject.transform.position += new Vector3(x,y,0.0f)*Time.deltaTime;
		MapGen.Dir dir=MovedToDoor();
		if(dir!=MapGen.Dir.Null&&level!=null)
			level.Move(dir);
    }

	private MapGen.Dir MovedToDoor(){
		Vector3 v=gameObject.transform.position;
		if(v.y<2.5&&v.y>-2.5&&v.x>-9.3&&v.x<9.3) return MapGen.Dir.Null;
		if((v.y>2.5)&&v.x<1&&v.x>-1){
			gameObject.transform.position = new Vector3(v.x,-2.51f,0.0f)*Time.deltaTime;
			return MapGen.Dir.Up;
		}
		if((v.y<-2.5)&&v.x<1&&v.x>-1){
			gameObject.transform.position = new Vector3(v.x,2.49f,0.0f)*Time.deltaTime;
			return MapGen.Dir.Down;
		}
		if((v.x>9.3)&&v.y<1&&v.y>-1){
			gameObject.transform.position = new Vector3(9.29f,v.y,0.0f)*Time.deltaTime;
			return MapGen.Dir.Right;
		}
		if((v.x<-9.3)&&v.y<1&&v.y>-1){
			gameObject.transform.position = new Vector3(-9.31f,v.y,0.0f)*Time.deltaTime;
			return MapGen.Dir.Left;
		}
		return MapGen.Dir.Null;
	}

    public void TakeDamage(int amount) {
        _health -= amount;
    }

    public void Heal(){
        _health = _maxHealth;
    }

    public void AddItem(ItemController i){
        _items.Add(i);
    }

    public void SelectItem(string name){
        foreach (var item in _items){
            
        }
    }

    public static int GetLevel()
    {
        return (int)(_level);
    }
}
