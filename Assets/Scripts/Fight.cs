using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Fight : MonoBehaviour
{
	private float timer;
	private bool fightText;
    private int fightTextOpacity = 99;

    public static bool _isBossFight;

    public GameObject oger;
    public GameObject fluffy;
    
    public Image healthBarPlayer;
    public Image healthBarEnemy;
	public GameObject weaponField;
	public TextMeshProUGUI text;
    
    void Start() {
		//Lock player movement in Level Scene
        PlayerController.MovementLocked(true);
		//Make a random enemy appear
        if (Random.Range(-1.0f, 1.0f) > 0.0) {
            oger.SetActive(true);
        } else {
            fluffy.SetActive(true);
        }
        //Show intro text for 2 seconds
		timer = 1.5f;
		fightText = true;
        if(_isBossFight)
		    text.SetText("<#8B0000>Boss Fight!!!");
        else
            text.SetText("<#8B0000>Fight!");

        GameObject.Find("Player").GetComponent("PlayerController");
    }

    private void EndFight() {
        PlayerController.MovementLocked(false);
        SceneManager.UnloadSceneAsync("Fight");
    }
    
    void Update() {
        healthBarPlayer.fillAmount = (float) (PlayerController.GetHealth() / PlayerController.GetMaxHealth());
        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            if (fightText)
		        FightText();
        }
    }
	
	//Makes fight text disappear
	private void FightText() {
        if (fightTextOpacity > 10) {
		    fightTextOpacity -= 1;
            if (_isBossFight)
                text.SetText($"<#8B0000{fightTextOpacity}>Boss Fight!!!");
            else
                text.SetText($"<#8B0000{fightTextOpacity}>Fight!");
        } else {
            text.SetText("<#FFFFFF00>Placeholder");
            fightText = false;
            weaponField.SetActive(true);
        }
	}
}