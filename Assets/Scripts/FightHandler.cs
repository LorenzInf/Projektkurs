using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightHandler : MonoBehaviour{

	public GameObject boss;
	public GameObject enemy;
	public GameObject player;
	
	private GameObject current;

	public void Awake(){
		SetUpFight();
	}

	public void SetUpFight(){
	    if(FightConfig.IsBoss())
	    {
		    current = Instantiate(boss);
	    }else{
		    current = Instantiate(enemy);
	    }
    }

    public void HandleInput(string s){
	    s = s.ToLower();
	    switch (s)
	    {
		    case "win":
			    EndFight();
			    break;
	    }
    }

	public void EndFight(){
		if(FightConfig.IsBoss()){
			SceneManager.LoadScene("Hub");
		}else{
			SceneManager.LoadScene("Level");
		}
	}
}
