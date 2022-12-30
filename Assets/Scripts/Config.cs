using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour{
    
    private static bool _soundOn = true;
    private static bool _musicOn = true;

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
}
