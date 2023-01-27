using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public bool isBoss;

    private float time=0,attackTime=0;
    private double health = 0,damage=0;
    private int level=0;
    private GameObject player;

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
        player = GameObject.Find("Player");
    }

    public void TakeDamage(double damage){
        health -= damage;
    }

    public bool Lifes(){
        return health > 0;
    }

    public void Update(){
        time += Time.deltaTime;
        if (time>=attackTime){
            if (CanReach())
            {
                GameObject go=GameObject.Find("Main Camera");
                if (go != null)
                    (go.GetComponent("LevelController") as LevelController).AttackPlayer(damage);
                time = 0f;
            }
        }
        Vector3 v = player.transform.position - gameObject.transform.position;
        Vector3 n = Vector3.Normalize(v);
        gameObject.transform.position += n * 2 * Time.deltaTime;
    }

    public bool CanReach() {
        Vector3 v = player.transform.position - gameObject.transform.position;
        return v.magnitude < 1;
    }

}