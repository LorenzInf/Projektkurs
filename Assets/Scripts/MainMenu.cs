using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour {

    private GameObject _mainMenu;
    private GameObject _optionsMenu;
    private GameObject _soundIcon;
    private GameObject _soundIconOff;
    private GameObject _musicIcon;
    private GameObject _musicIconOff;

    public Config config;

    public AudioSource audioSource;

    void Start() {
        _mainMenu = GameObject.Find("MainMenu");
        _optionsMenu = GameObject.Find("OptionsMenu");
        _soundIcon = GameObject.Find("SoundButton/sound_icon");
        _soundIconOff = GameObject.Find("SoundButton/sound_icon_off");
        _musicIcon = GameObject.Find("MusicButton/music_icon");
        _musicIconOff = GameObject.Find("MusicButton/music_icon_off");
        
        _optionsMenu.SetActive(false);
        _soundIconOff.SetActive(false);
        _musicIconOff.SetActive(false);
    }

    public void PlayGame () {
        SceneManager.LoadScene("Hub");
    }

    public void PlayClick () {
        if(config.GetSoundOn())
            audioSource.Play();
    }

    public void ShowOptions () {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void QuitGame () {
        Debug.Log("[DEBUG] The game was quit");
        Application.Quit();
    }

    public void ReadInput (string s) {
        Debug.Log("[DEBUG] The input was: \"" + s + "\"");
        if (s.ToLower() == "quit") {
            QuitGame();
        } else if (s.ToLower() == "options") {
            ShowOptions();
        } else if (s.ToLower() == "play") {
            PlayGame();
        }
    }

    public void ToggleSound () {
        _soundIcon.SetActive(!_soundIcon.activeSelf);
        _soundIconOff.SetActive(!_soundIconOff.activeSelf);
        config.SetSound(!config.GetSoundOn());
    }

    public void ToggleMusic () {
        _musicIcon.SetActive(!_musicIcon.activeSelf);
        _musicIconOff.SetActive(!_musicIconOff.activeSelf);
        config.SetMusic(!config.GetMusicOn());
    }
}
