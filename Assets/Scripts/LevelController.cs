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
	private static int cx=-1,cy=-1;
	private static MapGen.Room?[,] r=null;

	void Start(){
		player=GameObject.Find("Player");
		if(r==null)
			SetUpLevel();
		SetRoom();
	}

	public void SetUpLevel(){
		r=MapGen.Gen(5,5,5);
		for(int i=0;i<5;i++){
			for(int j=0;j<5;j++){
				if(r[i,j].ToString().Contains("Starting")){
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

	public void SetRoom(){
		MapGen.Room? room = r[cx,cy];
		foreach (GameObject go in currentRoom){
			Destroy(go);
		}
		currentRoom.Clear();
		string s = room.ToString();
		if(s.Contains("Boss")){
			r[cx,cy]?.SetRoomType(MapGen.RoomType.Empty);
			CreateFight(true);
		}else if(s.Contains("Enemy")){
			Debug.Log(r[cx,cy]?.SetRoomType(MapGen.RoomType.Empty));
			CreateFight(false);
		}else{
			currentRoom.Add(Instantiate(emptyroomPrefab));
			if(s.Contains("^"))
				currentRoom.Add(Instantiate(doorClosedPrefab));
			if (s.Contains("v")){
				GameObject go = Instantiate(doorClosedPrefab);
				go.transform.position = new Vector3(0f,-3.7f,-1f);
				currentRoom.Add(go);
			}
			if(s.Contains(">"))
				currentRoom.Add(Instantiate(sideDoorClosedPrefab));
			if (s.Contains("<")){
				GameObject go = Instantiate(sideDoorClosedPrefab);
				go.transform.position = new Vector3(-10.2f,0f,0f);
				currentRoom.Add(go);
			}
			if (s.Contains("Loot")) {
				if (room?.GetItems() != null) {
					currentRoom.Add(Instantiate(ChestClosedPrefab));
				}else {
					currentRoom.Add(Instantiate(ChestOpenPrefab));
				}
			}
		}
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

	public void GetLoot() {
		r[cx,cy]?.TakeItems();
	}

	public void CreateFight(bool boss){
		FightConfig.SetBoss(boss);
		Destroy(gameObject);
		SceneManager.LoadScene("Fight");
	}
}
