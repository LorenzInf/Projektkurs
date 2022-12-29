using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    public string name;
    public bool usesAmmo;
    public int ammo;
    public int durability;
    private int currentDurability;

    public void Start(){
        currentDurability = durability;
    }

    public void Use(){
        if (CanBeUsed()) {
            if (usesAmmo)
                ammo--;
            currentDurability--;
            if(currentDurability==0)
                Destroy(this);
        }else{
            Destroy(this);
        }
    }

    public bool CanBeUsed(){
        if (currentDurability < 1)
            return false;
        if (usesAmmo)
            return ammo > 0;
        return true;
    }

    public void Repair(int ammount){
        currentDurability += ammount;
        if (currentDurability > durability)
            currentDurability = durability;
    }
}
