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
	private Attack currentAttack=null;

	public void Awake(){
		SetUpFight(PlayerController.inBossFight);
	}

	public void Update(){
		if(currentAttack!=null){
			currentAttack.time+=Time.deltaTime;
			if(currentAttack.time>=10-0.2*currentAttack.level)
				EndAttack();
		}
	}
    
    public void SetUpFight(bool boss){
        
    }

    public void HandleInput(string s){
        
    }

	public void EndFight(){
		SceneManager.LoadScene("Level");
	}

	public void StartAttack(int lvl){
		currentAttack=new Attack(lvl);
	}

	public void EndAttack(){
		currentAttack=null;
	}

	public class Attack{
		public bool blocked=false;
		public int level;
		public float time=0;

		public Attack(int lvl){
			level=lvl;
		}
	}
}
