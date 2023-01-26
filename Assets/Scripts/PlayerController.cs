using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
	
	public enum Item {
		AmmoBox,
		HealingPotion,
		RepairKit,
		Null
	}
    
    private static List<Item> _items=new List<Item>();
    private static Dictionary<string, WeaponController> _weapons = new Dictionary<string,WeaponController>();
    private static double _maxHealth=100;
    private static double _health=100;
    private static double _level=1;
    private static int _rugh = 0;

    private bool left=false;

    public GameObject player;
    public LevelController level=null;
	public bool canMove;

    public void Update(){
		if(canMove)
        	HandleMovement();
    }

    public void HandleMovement(){
		float x=0.0f,y=0.0f;
        if (Input.GetKey(KeyCode.DownArrow))
            y-=4;
        if (Input.GetKey(KeyCode.LeftArrow))
        	x-=4;
        if (Input.GetKey(KeyCode.UpArrow))
            y+=4;
        if (Input.GetKey(KeyCode.RightArrow))
            x+=4;
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

	private void ValidatePosition() {
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

	public void Reset(){
		_items=new List<Item>();
		_weapons = new Dictionary<string,WeaponController>();
		AddWeapon(WeaponController.CreateNewWeapon("Branch","Branch"));
		Heal();
	}

	public bool Lifes(){
		return _health > 0;
	}

    public void TakeDamage(double amount) {
        _health -= amount;
    }

    public void Heal(){
        _health = _maxHealth;
    }

    public void AddItem(Item i){
        _items.Add(i);
    }
    
    public void AddWeapon(WeaponController w){
	    if (GetWeapon(w.GetName()) != null){
		    _weapons.Remove(w.GetName());
	    }
	    _weapons.Add(w.GetName(),w);
    }

    public WeaponController GetWeapon(string name){
	    if(_weapons.ContainsKey("name")){  
		    return _weapons["name"];
	    }else {
		    return null;
	    }
    }

    public static int GetLevel() {
        return (int)(_level);
    }

    public static void LevelUp(double amount){
	    _level += amount / (_level/2);
    }

    public static void AddRugh(int amount) {
	    _rugh += amount;
    }

    public static int GetRugh(){
	    return _rugh;
    }
    public void UseItem(string item,WeaponController wc){
	    switch (item) {
		    case "healingpotion":
			    _health = _maxHealth;
			    break;
		    case "ammobox":
			    if (wc!=null)
				    wc.Refill();
			    break;
		    case "repairkit":
			    if (wc!=null)
				    wc.Repair();
			    break;
	    }
    }

    public static bool Purchasable(string upgrade,bool purchase){
	    bool purchaseable = false;
	    switch (upgrade){
		    case "health":
			    purchaseable = ((_maxHealth - 100) / 100) < 3;
			    if (purchaseable)
				    purchaseable = _rugh >= ((_health - 100) / 100) * 5;
			    if (purchase && purchaseable){
				    _rugh -= (int)(((_maxHealth - 100) / 100) * 5);
				    _maxHealth += 100;
			    }
			    break;
	    }
	    return purchaseable;
    }
}
