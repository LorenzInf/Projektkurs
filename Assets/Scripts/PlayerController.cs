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
            y-=3;
        if (Input.GetKey(KeyCode.A))
        	x-=3;
        if (Input.GetKey(KeyCode.W))
            y+=3;
        if (Input.GetKey(KeyCode.D))
            x+=3;
		if(left==x>0&&x!=0.0f){
			gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
			left=!left;
		}
		gameObject.transform.position += new Vector3(x,y,0.0f)*Time.deltaTime;
		MapGen.Dir dir=MovedToDoor();
		if (dir != MapGen.Dir.Null)
			level.Move(dir);
		ValidatePosition();
    }

	private MapGen.Dir MovedToDoor(){
		Vector3 v=gameObject.transform.position;
		if (level == null) return MapGen.Dir.Null;
		if(v.y<2.5&&v.y>-2.5&&v.x>-9.3&&v.x<9.3) return MapGen.Dir.Null;
		if((v.y>2.5)&&v.x<1&&v.x>-1){
			if (level.CanMove(MapGen.Dir.Up)){
				gameObject.transform.position = new Vector3(v.x,-2.49f,v.z)*Time.deltaTime;
				return MapGen.Dir.Up;
			}
		}else if((v.y<-2.5)&&v.x<1&&v.x>-1){
			if (level.CanMove(MapGen.Dir.Down)){
				gameObject.transform.position = new Vector3(v.x, 2.49f, v.z) * Time.deltaTime;
				return MapGen.Dir.Down;
			}
		}else if((v.x>9.3)&&v.y<1&&v.y>-1){
			if (level.CanMove(MapGen.Dir.Right)){
				gameObject.transform.position = new Vector3(9.29f, v.y, v.z) * Time.deltaTime;
				return MapGen.Dir.Right;
			}
		}else if((v.x<-9.3)&&v.y<1&&v.y>-1){
			if (level.CanMove(MapGen.Dir.Left)){
				gameObject.transform.position = new Vector3(-9.29f, v.y, v.z) * Time.deltaTime;
				return MapGen.Dir.Left;
			}
		}
		return MapGen.Dir.Null;
	}

	private void ValidatePosition()
	{
		Vector3 v=gameObject.transform.position;
		if (v.x<-9.4){
			gameObject.transform.position = new Vector3(-9.39f,v.y,v.z);
		}
		if (v.x>9.4){
			gameObject.transform.position = new Vector3(9.39f,v.y,v.z);
		}
		if (v.y<-2.6){
			gameObject.transform.position = new Vector3(v.x,-2.59f,v.z);
		}
		if (v.y>2.6){
			gameObject.transform.position = new Vector3(v.x,2.59f,v.z);
		}
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
