using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour{
    
    private static bool _soundOn = true;
    private static bool _musicOn = true;
    private static List<Weapon> aWeapons = new List<Weapon>();
    private static List<Item> aItems = new List<Item>();

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

    public static bool IsAvailabel(Item i){
        foreach (Item VARIABLE in aItems){
            if (VARIABLE.ToString() == i.ToString())
                return true;
        }
        return false;
    }
    
    public static bool IsAvailabel(Weapon w){
        foreach (Weapon VARIABLE in aWeapons){
            if (VARIABLE.ToString() == w.ToString())
                return true;
        }
        return false;
    }

    public static void MakeAvailabel(Weapon w) {
        if (!IsAvailabel(w))
            aWeapons.Add(w);
    }
    
    public static void MakeAvailabel(Item i) {
        if (!IsAvailabel(i))
            aItems.Add(i);
    }

    public static Item GetRandomItem(){
        int i = aItems.Count;
        if (i == 0)
            MakeAvailabel(Item.AmmoBox);
        i=(int)(Random.Range(0,i));
        return aItems[i];
    }

    public static Weapon GetRandomWeapon(){
        int i = aWeapons.Count;
        if (i == 0)
            MakeAvailabel(Weapon.Dagger);
        i=(int)(Random.Range(0,i));
        return aWeapons[i];
    }
    
    public enum Item{
        AmmoBox,
        HealingPotion,
        RepairKit,
        Null
    }
    
    public enum Weapon{
        Baseballbat,
        Bow,
        Dagger,
        Sword,
        Machete,
        Pistol,
        Spear,
        Scythe,
        Null
    }
}