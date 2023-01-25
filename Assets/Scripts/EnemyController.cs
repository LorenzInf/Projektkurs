using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public bool isBoss;

    private float time=0,attackTime=0;
    private double health = 0,damage=0;
    private int level=0;

    public void Awake(){
        if (isBoss){
            level = (int)(PlayerController.GetLevel() / 1.2);
        }else {
            level = (int)(PlayerController.GetLevel() / 2);
        }
        if (level <= 0) level = 1;
        health = level * 50;
        damage = level * 10;
        attackTime = 12 - level/2;
        if (attackTime < 1) attackTime = 1;
    }

    public void TakeDamage(double damage){
        health -= damage;
    }

    public void Update(){
        time += Time.deltaTime;
        if (time >= attackTime){
            GameObject go=GameObject.Find("Main Camera");
            if (go != null)
                (go.GetComponent("FightHandler") as FightHandler).AttackPlayer(damage);
            time = 0f;
        }
    }

}
