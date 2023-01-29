using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

	private static List<Config.Item> _items=new List<Config.Item>();
	private static Dictionary<string, WeaponController> _weapons = new Dictionary<string, WeaponController>();
    private static double _maxHealth=100;
    public static double _health=100;
    public static double _level=1;
    public static int _rugh = 0;
	public static int _tempRugh = 0;
    private static bool _movementLocked = false;
    
    private bool left=false;
    private GameObject current=null;
	private float scale;

    public GameObject player;
	public GameObject inventar;
    public LevelController level=null;
	public bool canMove;
	public GameObject[] weapons;

	public void Start(){
		scale=Camera.main.orthographicSize / 6;
		AddWeapon(WeaponController.CreateWeapon(Config.Weapon.Bat));
	}

    public void Update(){
		if(canMove)
        	HandleMovement();
    }

    public void HandleMovement(){
	    if (!_movementLocked) {
		    float x = 0.0f, y = 0.0f;
		    if (Input.GetKey(KeyCode.DownArrow))
			    y -= 6 * scale;
		    if (Input.GetKey(KeyCode.LeftArrow))
			    x -= 6 * scale;
		    if (Input.GetKey(KeyCode.UpArrow))
			    y += 6 * scale;
		    if (Input.GetKey(KeyCode.RightArrow))
			    x += 6 * scale;
		    if (left == x > 0 && x != 0.0f) {
			    gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
			    if (current != null)
				    current.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
			    left = !left;
		    }

		    gameObject.transform.position += new Vector3(x, y, 0.0f) * Time.deltaTime;
		    MapGen.Dir dir = MovedToDoor();
		    if (dir != MapGen.Dir.Null)
			    level.Move(dir);
		    ValidatePosition();
		    if (current != null)
			    current.transform.position = gameObject.transform.position;
	    }
    }

	private MapGen.Dir MovedToDoor(){
		Vector3 v=gameObject.transform.position;
		if (level == null) return MapGen.Dir.Null;
		if(v.y<2.5*scale&&v.y>-2.5*scale&&v.x>-9.3*scale&&v.x<9.3*scale) return MapGen.Dir.Null;
		if((v.y>2.5*scale)&&v.x<1*scale&&v.x>-1*scale){
			if (level.CanMove(MapGen.Dir.Up)){
				gameObject.transform.position = new Vector3(v.x,-2.49f*scale,v.z);
				return MapGen.Dir.Up;
			}
		}else if((v.y<-2.5*scale)&&v.x<1*scale&&v.x>-1*scale){
			if (level.CanMove(MapGen.Dir.Down)){
				gameObject.transform.position = new Vector3(v.x, 2.49f*scale, v.z);
				return MapGen.Dir.Down;
			}
		}else if((v.x>9.01*scale)&&v.y<1*scale&&v.y>-1*scale){
			if (level.CanMove(MapGen.Dir.Right)){
				gameObject.transform.position = new Vector3(-9f*scale, v.y, v.z);
				return MapGen.Dir.Right;
			}
		}else if((v.x<-9.01*scale)&&v.y<1*scale&&v.y>-1*scale){
			if (level.CanMove(MapGen.Dir.Left)){
				gameObject.transform.position = new Vector3(9f*scale, v.y, v.z);
				return MapGen.Dir.Left;
			}
		}
		return MapGen.Dir.Null;
	}

	private void ValidatePosition() {
		Vector3 v=gameObject.transform.position;
		if (v.x<-9.3*scale){
			gameObject.transform.position = new Vector3(-9.09f*scale,v.y,v.z);
		}
		if (v.x>9.3*scale){
			gameObject.transform.position = new Vector3(9.09f*scale,v.y,v.z);
		}
		if (v.y<-2.6*scale){
			gameObject.transform.position = new Vector3(v.x,-2.59f*scale,v.z);
		}
		if (v.y>2.6*scale){
			gameObject.transform.position = new Vector3(v.x,2.59f*scale,v.z);
		}
		if (v.y<1*scale&&v.y>-1*scale&&v.x<1*scale&&v.x>-1*scale) {
			level.GetLoot();
		}
	}

	public void Reset(){
		_items=new List<Config.Item>();
		_weapons = new Dictionary<string,WeaponController>();
		AddWeapon(WeaponController.CreateWeapon(Config.Weapon.Bat));
		Heal();
		_movementLocked = false;
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
		(inventar.GetComponent("InventarController") as InventarController).MakeAvailabel(i);
        _items.Add(i);
    }
    
    public void AddWeapon(WeaponController w){
	    if (w != null) {
		    if (GetWeapon(w.GetName()) != null) {
			    _weapons.Remove(w.GetName());
		    }
			(inventar.GetComponent("InventarController") as InventarController).MakeAvailabel(w.GetType());
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
			Debug.Log(weapon.name.ToLower());
		    if (weapon.name.ToLower() == type) {
			    Destroy(current);
			    current = Instantiate(weapon);
				if(left)
					current.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
		    }
	    }
    }

    public static double GetMaxHealth() {
	    return _maxHealth;
    }

    public static double GetHealth() {
	    return _health;
    }

    public static void MovementLocked(bool locked) {
	    _movementLocked = locked;
    }
}