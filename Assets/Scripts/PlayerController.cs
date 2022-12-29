using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    
    private static float speed=1;
    private static ArrayList items;
    private static ItemController inHand;
    
    public static double health;
    public GameObject player;

    public void Start(){
        items = new ArrayList();
    }

    public void Update(){
        HandleMovement();
    }

    public void HandleMovement(){
        float x=0f, y=0f;
        if (Input.GetKey(KeyCode.W))
            y++;
        if (Input.GetKey(KeyCode.A))
            x--;
        if (Input.GetKey(KeyCode.S))
            y--;
        if (Input.GetKey(KeyCode.D))
            x++;
        Debug.Log(x+" "+y);
        player.transform.position += new Vector3(x,y,0f) * speed * Time.deltaTime;
    }

    public void TakeDamage(int amount) {
        health -= amount;
    }

    public void AddItem(ItemController i){
        items.Add(i);
    }

    public void SelectItem(string name){
        foreach (var item in items){
            
        }
    }
}
