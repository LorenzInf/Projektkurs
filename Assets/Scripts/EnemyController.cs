using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public bool isBoss;

    private float time=0;
    private double health=0;
    private int level=0;

    public void Awake(){
        if (isBoss){
            level = (int)(PlayerController.GetLevel() / 1.3);
        }else {
            level = (int)(PlayerController.GetLevel() / 2);
        }
        if (level <= 0) level = 1;
        health = level * 50;
    }

    public void TakeDamage(double damage){
        health -= damage;
    }

    public void Update(){
        time += Time.deltaTime;
        if (time >= 10.0f){
            GameObject go=GameObject.Find("Main Camera");
            //if (go != null)
                //(go.GetComponent("FightHandler") as FightHandler).StartAttack(level);
            time = 0f;
        }
    }

}
