using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static double health;

    public void TakeDamage(int amount) {
        health -= amount;
    }
}
