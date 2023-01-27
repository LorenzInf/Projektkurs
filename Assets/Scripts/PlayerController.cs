using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

	private static List<Config.Item> _items=new List<Config.Item>();
	private static Dictionary<string, WeaponController> _weapons = new Dictionary<string, WeaponController>();
    private static double _maxHealth=100;
    private static double _health=100;
    private static double _level=1;
    private static int _rugh = 0;

    private bool left=false;
    private GameObject current=null;

    public GameObject player;
    public LevelController level=null;
	public bool canMove;
	public GameObject[] weapons;

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
			if(current!=null)
				current.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
			left=!left;
		}
		gameObject.transform.position += new Vector3(x,y,0.0f)*Time.deltaTime;
		MapGen.Dir dir=MovedToDoor();
		if (dir != MapGen.Dir.Null)
			level.Move(dir);
		ValidatePosition();
		if(current!=null)
			current.transform.position = gameObject.transform.position;
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
		if (v.y<1&&v.y>-1&&v.x<1&&v.x>-1) {
			level.GetLoot();
		}
	}

	public void Reset(){
		_items=new List<Config.Item>();
		_weapons = new Dictionary<string,WeaponController>();
		AddWeapon(WeaponController.CreateWeapon(Config.Weapon.Basballbat));
		Heal();
	}

	public double Attack(WeaponController w,double dist) {
		SetLastWeapon(w);
		if(dist<w.GetRange())
			return w.Use();
		return 0;
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

    public void AddItem(Config.Item i){
        _items.Add(i);
    }
    
    public void AddWeapon(WeaponController w){
	    if (w != null) {
		    if (GetWeapon(w.GetName()) != null) {
			    _weapons.Remove(w.GetName());
		    }
		    _weapons.Add(w.GetName(), w);
	    }
    }

    public WeaponController GetWeapon(string name){
	    if (_weapons.ContainsKey(name)){
		    return _weapons[name];
	    }else {
		    return null;
	    }
    }

    public static int GetLevel() {
        return (int)(_level);
    }

    public static void LevelUp(){
	    _level++;
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

    public void SetLastWeapon(WeaponController w) {
	    string type = w.GetType().ToString().ToLower();
	    foreach (var weapon in weapons) {
		    if (weapon.name.ToLower() == type) {
			    Destroy(current);
			    current = Instantiate(weapon);
		    }
	    }
    }

    public static double GetMaxHealth() {
	    return _maxHealth;
    }
}