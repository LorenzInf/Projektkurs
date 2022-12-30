using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemController : MonoBehaviour {

    public string itemname;
    public bool usesAmmo;
    public int ammo;
    public int durability;
    private int _currentDurability;

    public void Start(){
        _currentDurability = durability;
    }

    public void Use(){
        if (CanBeUsed()) {
            if (usesAmmo)
                ammo--;
            _currentDurability--;
            if(_currentDurability==0)
                Destroy(this);
        }else{
            Destroy(this);
        }
    }

    public bool CanBeUsed(){
        if (_currentDurability < 1)
            return false;
        if (usesAmmo)
            return ammo > 0;
        return true;
    }

    public void Repair(int ammount){
        _currentDurability += ammount;
        if (_currentDurability > durability)
            _currentDurability = durability;
    }
}
