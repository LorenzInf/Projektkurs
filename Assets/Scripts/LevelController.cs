using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour{

    public GameObject player;
	public GameObject emptyroomPrefab;
	public GameObject doorOpenPrefab;
	public GameObject doorClosePrefab;
	public GameObject sideDoorOpenPrefab;
	public GameObject sideDoorClosePrefab;

	private List<GameObject> currentRoom = new List<GameObject>();
	private int cx=-1,cy=-1,height=0,width=0;
	private MapGen.Room?[,] r=null;

	void Awake(){
		DontDestroyOnLoad(this);
		CreateLevel(5,5,5);
		SetRoom(r[cx,cy]);
	}

	public void CreateLevel(int height,int width,int amountOfRooms){
		r=MapGen.Gen(height,width,amountOfRooms);
		this.height=height;
		this.width=width;
		for(int i=0;i<height;i++){
			for(int j=0;j<height;j++){
				if(r[i,j].ToString().Contains("Starting")){
					cx=i;
					cy=j;
				}
			}
		}
	}

	public void Move(MapGen.Dir dir){
		int x=-1,y=-1;
		if(dir==MapGen.Dir.Up){
			x=cx;
			y=cy-1;
		}else if(dir==MapGen.Dir.Down){
			x=cx;
			y=cy+1;
		}else if(dir==MapGen.Dir.Left){
			x=cx-1;
			y=cy;
		}else if(dir==MapGen.Dir.Right){
			x=cx+1;
			y=cy;
		}
		if(IsValid(x,y)){
			cx=x;
			cy=y;
			SetRoom(r[cx,cy]);
		}
	}

	public bool IsValid(int x,int y){
		return x>=0&&x<width&&y>=0&&y<height&&r[x,y]!=null;
	}

	public void SetRoom(MapGen.Room? room) {
		foreach (GameObject go in currentRoom){
			Destroy(go);
		}
		currentRoom.Clear();
		if(room.ToString().Contains("Boss")){
			CreateFight(true);
		}else if(room.ToString().Contains("Enemy")){
			CreateFight(false);
		}else{
			currentRoom.Add(Instantiate(emptyroomPrefab));
			string s = room.ToString();
			if(s.Contains("^"))
				currentRoom.Add(Instantiate(doorClosePrefab));
			if (s.Contains("v")){
				GameObject go = Instantiate(doorClosePrefab);
				go.transform.position = new Vector3(0f,-3.7f, 0f);
				currentRoom.Add(go);
			}
			if(s.Contains(">"))
				currentRoom.Add(Instantiate(sideDoorClosePrefab));
			if (s.Contains("<")){
				GameObject go = Instantiate(sideDoorClosePrefab);
				go.transform.position = new Vector3(-10.2f,0f, 0f);
				currentRoom.Add(go);
			}
		}
		Debug.Log(room.ToString());
	}

	public void GetLoot(){
		
	}

	public void CreateFight(bool boss){
		if(boss)
			PlayerController.inBossFight=true;
		SceneManager.LoadScene("Fight");
	}

	public void EndFight(){
		SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
	}
}
