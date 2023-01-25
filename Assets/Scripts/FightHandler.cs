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

	    var w = PlayerController.GetWeapon(s);
	    if (w != null) {
		    if (w.CanBeUsed()) {
			    double value = w.Use();
			    // ???
		    } else {
			    // ???
		    }
	    } else {
		    // auf xaver warten
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
