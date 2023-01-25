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

	private static List<GameObject> currentRoom = new List<GameObject>();
	private static int cx = -1,cy = -1;
	private static MapGen.Room[,] r = null;

	void Start(){
		player=GameObject.Find("Player");
		if(r==null)
			SetUpLevel(5,5,5);
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
		}else{
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
				currentRoom.Add(Instantiate(ChestOpenPrefab));
			}
		}
		room.Visited(true);
		Debug.Log("x="+cx+" y="+cy);
		Debug.Log(room.ToString());
	}

	public void HandleInput(string s)
	{
		s = s.ToLower();
		switch (s)
		{
			case "get loot":
				GetLoot();
				break;
		}
	}

	private void GetLoot() {
		r[cx,cy].TakeItems();
	}

	private void CreateFight(bool boss){
		FightConfig.SetBoss(boss);
		Destroy(gameObject);
		SceneManager.LoadScene("Fight");
	}
}
