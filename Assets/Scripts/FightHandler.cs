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
	private bool bossFight;

	public void Awake(){
		SetUpFight();
	}

	public void SetUpFight(){
		bossFight=FightConfig.IsBoss();
	    if(bossFight){
		    current = Instantiate(boss);
	    }else{
		    current = Instantiate(enemy);
	    }
    }

    public void HandleInput(string s){
	    s = s.ToLower();
	    var w = (player.GetComponent("PlayerController") as PlayerController).GetWeapon(s);
	    if (w != null) {
		    double damage = w.Use();
			if(bossFight){
				(boss.GetComponent("EnemyController") as EnemyController).TakeDamage(damage);
			}else{
				(enemy.GetComponent("EnemyController") as EnemyController).TakeDamage(damage);
			}
	    } else {
			string[] st=s.Split(' ');
			w=(player.GetComponent("PlayerController") as PlayerController).GetWeapon(st[1]);
			if(w!=null)
				(player.GetComponent("PlayerController") as PlayerController).UseItem(st[0],w);
	    }
    }

	public void AttackPlayer(double damage){
		(player.GetComponent("PlayerController") as PlayerController).TakeDamage(damage);
	}

	public void EndFight(){
		if(bossFight){
			(player.GetComponent("PlayerController") as PlayerController).AddRugh(PlayerController.GetLevel());
			(player.GetComponent("PlayerController") as PlayerController).LevelUp(PlayerController.GetLevel());
			SceneManager.LoadScene("Hub");
		}else{
			(player.GetComponent("PlayerController") as PlayerController).AddRugh(PlayerController.GetLevel()/5);
			(player.GetComponent("PlayerController") as PlayerController).LevelUp(PlayerController.GetLevel()/5);
			SceneManager.LoadScene("Level");
		}
	}
}
