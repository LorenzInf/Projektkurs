using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public bool isBoss;
    
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
    }

    public void TakeDamage(double damage){
        health -= damage;
    }

    public bool Lifes(){
        return health > 0;
    }

    public void Update(){
        
    }
}