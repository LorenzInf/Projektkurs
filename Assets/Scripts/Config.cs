using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour{
    
    private static bool _soundOn = true;
    private static bool _musicOn = true;
    private static bool[] items = { false, false, false };
    private static bool[] weapons = { false, false, false, false, false, false, false, false, false };

    public bool GetSoundOn(){
        return _soundOn;
    }

    public bool GetMusicOn(){
        return _musicOn;
    }

    public void SetSound(bool sound){
        _soundOn = sound;
    }

    public void SetMusic(bool music){
        _musicOn = music;
    }

    public static bool ItemAvailable(int i){
        return items[i];
    }

    public static bool WeaponAvailable(int i) {
        return weapons[i];
    }

    public static void MakeItemAvailable(int i) {
        items[i] = true;
    }
    
    public static void MakeWeaponAvailable(int i) {
        weapons[i] = true;
    }
}
