using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour{
    
    private static bool soundOn = true;
    private static bool musicOn = true;

    public bool GetSoundOn(){
        return soundOn;
    }

    public bool GetMusicOn(){
        return musicOn;
    }

    public void SetSound(bool sound){
        soundOn = sound;
    }

    public void SetMusic(bool music){
        musicOn = music;
    }
}
