using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour{

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

	void Start() {
		int i = PlayerController.GetLevel() + 4;
		SetUpLevel(i, i, i);
		(player.GetComponent("PlayerController") as PlayerController).Reset();
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
		foreach (GameObject go in currentRoom){
			Destroy(go);
		}
		currentRoom.Clear();
		bool b = room.Visited(false);
		string s = room.ToString();
		if(s.Contains("Boss")&&!b){
			CreateFight(true);
		}else if(s.Contains("Enemy")&&!b){
			CreateFight(false);
		}
		currentRoom.Add(Instantiate(emptyroomPrefab));
		bool open = false;
		if(s.Contains("^")){
			open = r[cx,cy-1].Visited(false);
			if (open){
				currentRoom.Add(Instantiate(doorOpenPrefab));
			}else{
				currentRoom.Add(Instantiate(doorClosedPrefab));
			}
		}
		if (s.Contains("v")){
			open = r[cx,cy+1].Visited(false);
			if(open){
				GameObject go = Instantiate(doorOpenPrefab);
				go.transform.position = new Vector3(0f,-3.7f,-1f);
				currentRoom.Add(go);
			}else{
				GameObject go = Instantiate(doorClosedPrefab);
				go.transform.position = new Vector3(0f,-3.7f,-1f);
				currentRoom.Add(go);
			}
		}
		if (s.Contains(">")){
			open = r[cx+1,cy].Visited(false);
			if(open){
				currentRoom.Add(Instantiate(sideDoorOpenPrefab));
			}else{
				currentRoom.Add(Instantiate(sideDoorClosedPrefab));
			}
		}
		if (s.Contains("<")){
			open = r[cx-1,cy].Visited(false);
			if(open){
				GameObject go = Instantiate(sideDoorOpenPrefab);
				go.transform.position = new Vector3(-9.8f,-0.3f,0f);
				go.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
				currentRoom.Add(go);
			}else{
				GameObject go = Instantiate(sideDoorClosedPrefab);
				go.transform.position = new Vector3(-10.2f,0f,0f);
				currentRoom.Add(go);
			}
		}
		if (s.Contains("Loot")) {
			s = room.GetLoot();
			if (s.Contains("Null")){
				currentRoom.Add(Instantiate(ChestOpenPrefab));
			}else{
				currentRoom.Add(Instantiate(ChestClosedPrefab));
			}
		}
		room.Visited(true);
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
			//string[] st=s.Split(' ');
			//w=(player.GetComponent("PlayerController") as PlayerController).GetWeapon(st[1]);
			//if(w!=null)
			//	(player.GetComponent("PlayerController") as PlayerController).UseItem(st[0],w);
	    }
	}

	public void GetLoot() {
		MapGen.Room room = r[cx, cy];
		if(room.ToString().Contains("Loot")){
			string s = room.GetLoot();
			if (s.Contains("item")){
				(player.GetComponent("PlayerController") as PlayerController).AddItem(room.TakeItem());
			}else if (s.Contains("weapon")){
				(player.GetComponent("PlayerController") as PlayerController).AddWeapon(WeaponController.CreateWeapon(room.TakeWeapon()));
			}
			SetRoom();
		}
	}

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

	private void CreateFight(bool bossFight){
		if(bossFight){
			currentEnemy=Instantiate(boss);
		}else{
			currentEnemy=Instantiate(enemy);
		}
	}

	private void EndFight(){
		if(r[cx,cy].ToString().Contains("Boss")){
			PlayerController.AddRugh(PlayerController.GetLevel());
			PlayerController.LevelUp();
			SceneManager.LoadScene("Hub");
		}else{
			Destroy(currentEnemy);
			PlayerController.AddRugh(PlayerController.GetLevel()/5);
		}
	}
}
