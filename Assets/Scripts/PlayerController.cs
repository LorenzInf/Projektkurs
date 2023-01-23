using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    
    private static ArrayList _items=new ArrayList();
    private static ItemController _inHand=null;
    private static double _maxHealth=100;
    private static double _health=100;
    private static double _level=1;
    
    public GameObject player;
    public LevelController level=null;

    public void Start(){
        _items = new ArrayList();
        
    }

    public void Update(){
        HandleMovement();
    }

    public void HandleMovement(){
        if (level != null){
            if (Input.GetKeyDown(KeyCode.W))
                level.Move(MapGen.Dir.Up);
            if (Input.GetKeyDown(KeyCode.A))
                level.Move(MapGen.Dir.Left);
            if (Input.GetKeyDown(KeyCode.S))
                level.Move(MapGen.Dir.Down);
            if (Input.GetKeyDown(KeyCode.D))
                level.Move(MapGen.Dir.Right);
        }
    }

    public void TakeDamage(int amount) {
        _health -= amount;
    }

    public void Heal(){
        _health = _maxHealth;
    }

    public void AddItem(ItemController i){
        _items.Add(i);
    }

    public void SelectItem(string name){
        foreach (var item in _items){
            
        }
    }

    public static int GetLevel()
    {
        return (int)(_level);
    }
}
