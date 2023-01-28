using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour{
	public GameObject things;
    public GameObject player;
	public GameObject emptyroomPrefab;
	public GameObject doorOpenPrefab;
	public GameObject doorClosedPrefab;
	public GameObject sideDoorOpenPrefab;
	public GameObject sideDoorClosedPrefab;
	public GameObject ChestOpenPrefab;
	public GameObject ChestClosedPrefab;
	public GameObject enemy;
	public GameObject boss;

	private List<GameObject> currentRoom=new List<GameObject>();
	private GameObject currentEnemy=null;
	private int cx = -1,cy = -1;
	private MapGen.Room[,] r = null;
	private float scale;

	void Start() {
		scale=Camera.main.orthographicSize / 6;
		int i = PlayerController.GetLevel() + 4;
		SetUpLevel(i, i, i);
		SetRoom();
	}

	private void SetUpLevel(int width, int height, int amountOfRooms){
		r=MapGen.Gen(width,height,amountOfRooms);
		for(int i=0;i<width;i++){
			for(int j=0;j<height;j++){
				if(r[i,j]!=null&&r[i,j].ToString().Contains("Starting")){
					cx=i;
					cy=j;
				}
			}
		}
	}

	public void Move(MapGen.Dir dir){
		string s = r[cx,cy].ToString();
		if(dir==MapGen.Dir.Up&&s.Contains("^")){
			cy--;
		}else if(dir==MapGen.Dir.Down&&s.Contains("v")){
			cy++;
		}else if(dir==MapGen.Dir.Left&&s.Contains("<")){
			cx--;
		}else if(dir==MapGen.Dir.Right&&s.Contains(">")){
			cx++;
		}
		SetRoom();
	}

	public bool CanMove(MapGen.Dir dir){
		//if(currentEnemy!=null)
		//	return false;
		string s = r[cx,cy].ToString();
		if(dir==MapGen.Dir.Up&&s.Contains("^")){
			return true;
		}else if(dir==MapGen.Dir.Down&&s.Contains("v")){
			return true;
		}else if(dir==MapGen.Dir.Left&&s.Contains("<")){
			return true;
		}else if(dir==MapGen.Dir.Right&&s.Contains(">")){
			return true;
		}
		return false;
	}

	private void SetRoom(){
		MapGen.Room room = r[cx,cy];
		foreach (GameObject gameObject in currentRoom){
			Destroy(gameObject);
		}
		currentRoom.Clear();
		bool b = room.Visited(false);
		string s = room.ToString();
		GameObject go = Instantiate(emptyroomPrefab);
		go.transform.position *= scale;
		currentRoom.Add(go);
		bool open = false;
		if(s.Contains("^")){
			open = r[cx,cy-1].Visited(false);
			if (open){
				go = Instantiate(doorOpenPrefab);
				go.transform.position *= scale;
				currentRoom.Add(go);
			}else{
				go = Instantiate(doorClosedPrefab);
				go.transform.position *= scale;
				currentRoom.Add(go);
			}
		}
		if (s.Contains("v")){
			open = r[cx,cy+1].Visited(false);
			if(open){
				go = Instantiate(doorOpenPrefab);
				go.transform.position = new Vector3(0f,-4.88f,0f);
				currentRoom.Add(go);
			}else{
				go = Instantiate(doorClosedPrefab);
				go.transform.position = new Vector3(0f,-4.88f,0f);
				currentRoom.Add(go);
			}
		}
		if (s.Contains(">")){
			open = r[cx+1,cy].Visited(false);
			if(open){
				go = Instantiate(sideDoorOpenPrefab);
				go.transform.position *= scale;
				currentRoom.Add(go);
			}else{
				go = Instantiate(sideDoorClosedPrefab);
				go.transform.position *= scale;
				currentRoom.Add(go);
			}
		}
		if (s.Contains("<")){
			open = r[cx-1,cy].Visited(false);
			if(open){
				go = Instantiate(sideDoorOpenPrefab);
				go.transform.position = new Vector3(-9.63f*scale,0f,0f);
				go.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
				currentRoom.Add(go);
			}else{
				go = Instantiate(sideDoorClosedPrefab);
				go.transform.position = new Vector3(-9.63f*scale,0f,0f);
				currentRoom.Add(go);
			}
		}
		if (s.Contains("Loot")) {
			s = room.GetLoot();
			if (s.Contains("null")){
				go = Instantiate(ChestOpenPrefab);
				go.transform.position *= scale;
				currentRoom.Add(go);
			}else{
				go = Instantiate(ChestClosedPrefab);
				go.transform.position *= scale;
				currentRoom.Add(go);
			}
		}
		room.Visited(true);
		if(s.Contains("Boss")&&!b){
			Fight._isBossFight = true;
			things.SetActive(false);
			SceneManager.LoadScene("Fight", LoadSceneMode.Additive);
		} else if(s.Contains("Enemy")&&!b){
			Fight._isBossFight = false;
			things.SetActive(false);
			SceneManager.LoadScene("Fight", LoadSceneMode.Additive);
		}
	}

	public void HandleInput(string s) {
		s=s.ToLower();
		var w = (player.GetComponent("PlayerController") as PlayerController).GetWeapon(s);
	    if (w != null) {
			if(currentEnemy==null){
				(player.GetComponent("PlayerController") as PlayerController).Attack(w,0);
			}else{
				Vector3 v = player.transform.position - currentEnemy.transform.position;
				AttackEnemy((player.GetComponent("PlayerController") as PlayerController).Attack(w,v.magnitude));
			}
	    } else {
		    if (s == "healingpotion"){
			    (player.GetComponent("PlayerController") as PlayerController).UseItem(s, null);
		    }else{
			    string[] st=s.Split(' ');
			    if (st.Length > 1){
				    w=(player.GetComponent("PlayerController") as PlayerController).GetWeapon(st[1]);
				    if(w!=null)
					    (player.GetComponent("PlayerController") as PlayerController).UseItem(st[0],w);
			    }
		    }
	    }
	}

	public void GetLoot() {
		MapGen.Room room = r[cx, cy];
		if(room.ToString().Contains("Loot")){
			string s = room.GetLoot();
			if (s.Contains("item")){
				Debug.Log("item aufgehoben");
				(player.GetComponent("PlayerController") as PlayerController).AddItem(room.TakeItem());
			}else if (s.Contains("weapon")){
				Debug.Log("waffe aufgehoben");
				(player.GetComponent("PlayerController") as PlayerController).AddWeapon(WeaponController.CreateWeapon(room.TakeWeapon()));
			}
			SetRoom();
		}
	}

	/////
	public void AttackPlayer(double damage){
		Debug.Log((player.GetComponent("PlayerController") as PlayerController).Lifes());
		(player.GetComponent("PlayerController") as PlayerController).TakeDamage(damage);
		if(!(player.GetComponent("PlayerController") as PlayerController).Lifes()){
			EndFight();
		}
	}

	public void AttackEnemy(double damage){
		(currentEnemy.GetComponent("EnemyController") as EnemyController).TakeDamage(damage);
		if(!(currentEnemy.GetComponent("EnemyController") as EnemyController).Lifes()){
			EndFight();
		}
	}
	/////

	private void EndFight(){
		if(r[cx,cy].ToString().Contains("Boss")){
			PlayerController.AddRugh(PlayerController.GetLevel());
			PlayerController.LevelUp();
			SceneManager.LoadScene("Hub");
		}else{
			Destroy(currentEnemy);
			currentEnemy = null;
			PlayerController.AddRugh(PlayerController.GetLevel()/5);
		}
	}
}