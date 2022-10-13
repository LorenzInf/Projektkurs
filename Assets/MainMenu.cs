using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private GameObject mainMenu;
    private GameObject optionsMenu;
    private GameObject soundIcon;
    private GameObject soundIconOff;
    private GameObject musicIcon;
    private GameObject musicIconOff;


    void Start() {
        mainMenu = GameObject.Find("MainMenu");
        optionsMenu = GameObject.Find("OptionsMenu");
        soundIcon = GameObject.Find("SoundButton/sound_icon");
        soundIconOff = GameObject.Find("SoundButton/sound_icon_off");
        musicIcon = GameObject.Find("MusicButton/music_icon");
        musicIconOff = GameObject.Find("MusicButton/music_icon_off");

        optionsMenu.SetActive(false);
        soundIconOff.SetActive(false);
        musicIconOff.SetActive(false);
    }

    public void PlayGame () {
        //SceneManager.LoadScene("Level");
    }

    public void ShowOptions () {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void QuitGame () {
        Debug.Log("[DEBUG] The game was quit");
        Application.Quit();
    }

    public void ReadInput (string s) {
        Debug.Log("[DEBUG] The input was: \"" + s + "\"");
        if (s == "Quit" || s == "quit") {
            QuitGame();
        } else if (s == "Options" || s == "options") {
            ShowOptions();
        } else if (s == "Play" || s == "play") {
            PlayGame();
        }
    }

    public void ToggleSound () {
        soundIcon.SetActive(!soundIcon.activeSelf);
        soundIconOff.SetActive(!soundIconOff.activeSelf);
        //...
    }

    public void ToggleMusic () {
        musicIcon.SetActive(!musicIcon.activeSelf);
        musicIconOff.SetActive(!musicIconOff.activeSelf);
        //...
    }
}
