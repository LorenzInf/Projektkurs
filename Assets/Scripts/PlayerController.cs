using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static double health;
    private static ArrayList items = new ArrayList();
    private static ItemController inHand;

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
