using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    
    private static ArrayList _items;
    private static ItemController _inHand;
    private static double _health;
    
    public GameObject player;

    public void Start(){
        _items = new ArrayList();
        
    }

    public void Update(){
        HandleMovement();
    }

    public void HandleMovement(){
        float x=0f, y=0f;
        if (Input.GetKeyDown(KeyCode.W))
            y++;
        if (Input.GetKeyDown(KeyCode.A))
            x--;
        if (Input.GetKeyDown(KeyCode.S))
            y--;
        if (Input.GetKeyDown(KeyCode.D))
            x++;
        player.transform.position += new Vector3(x,y,0f);
    }

    public void TakeDamage(int amount) {
        _health -= amount;
    }

    public void AddItem(ItemController i){
        _items.Add(i);
    }

    public void SelectItem(string name){
        foreach (var item in _items){
            
        }
    }
}
