using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightConfig : MonoBehaviour
{
    private static bool _isBoss = false;
    
    public static bool IsBoss(){
        return _isBoss;
    }

    public static void SetBoss(bool isBoss){
        _isBoss = isBoss;
    }
}
