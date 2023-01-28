using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Fight : MonoBehaviour
{
	private float timer;
    private float timerMax;
    private float sinValue;
	private bool fightText;
    private bool attacking;
    private int fightTextOpacity = 99;
    private string[] allWords;
    private string currentWord;

    public static bool _isBossFight;

    public GameObject oger;
    public GameObject fluffy;
    public GameObject enemyObjects;
    private string enemyType;
    
    public Image healthBarPlayer;
    public Image healthBarEnemy;
	public TMP_InputField weaponField;
    public GameObject weaponFieldEmpty;
    public GameObject typeText;
	public TextMeshProUGUI text;
    public Image timerBar;
    
    void Start() {
        //Load words
        Resources.Load("Assets/Text/words.txt");
        allWords = System.IO.File.ReadAllLines("Assets/Text/words.txt");

		//Lock player movement in Level Scene
        PlayerController.MovementLocked(true);

		//Make a random enemy appear
        if (UnityEngine.Random.Range(-1.0f, 1.0f) > 0.0) {
            oger.SetActive(true);
            enemyType = "oger";
        } else {
            fluffy.SetActive(true);
            enemyType = "fluffy";
        }

        //Show intro text for 2 seconds
		timer = 1.5f;
		fightText = true;
        if(_isBossFight)
		    text.SetText("<#8B0000>Boss Fight!!!");
        else
            text.SetText("<#8B0000>Fight!");
    }

    private void EndFight() {
        PlayerController.MovementLocked(false);
        SceneManager.UnloadSceneAsync("Fight");
    }
    
    //Called when weaponField is deselected
    public void SelectWeapon() {
        WeaponController weapon = (GameObject.Find("Player").GetComponent("PlayerController") as PlayerController).GetWeapon(weaponField.text);
        if(weapon != null) {
            Debug.Log($"Weapon found: {weapon.GetName()}");
            attacking = true;
            weaponFieldEmpty.SetActive(false);
            typeText.SetActive(true);
            currentWord = allWords[(int) UnityEngine.Random.Range(0.0f, (float) allWords.Length)];
            text.SetText(currentWord);
        } else {
            weaponField.text = "Couldn't find that weapon...";
        }
    }

    void Update() {
        //Update player healthbar
        healthBarPlayer.fillAmount = (float) (PlayerController.GetHealth() / PlayerController.GetMaxHealth());

        //Count down timer
        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            if (fightText)
		        FightText();
        }

        //Set timerBar to percentage of timer while attacking
        if (attacking) {
            timerBar.fillAmount = timer / timerMax;
        }

        //Count up sinValue
        sinValue += Time.deltaTime;
        if (attacking) {
            //Move type text
            typeText.transform.position = new Vector2(0,(float) Math.Sin(4*sinValue)/7);
        }

        if(enemyType.Equals("fluffy")) {
            //Move fluffy & health bar
            Vector2 v = new Vector2((float) ((Math.Sin(2*sinValue + 2) + Math.Sin(sinValue))/2),(float) ((Math.Cos(2.5*sinValue) + Math.Sin(sinValue))/1.5));
            enemyObjects.transform.position = v;
        } else if (enemyType.Equals("oger")) {
            //Move oger & health bar
            Vector2 v = new Vector2((float) ((Math.Sin(2*sinValue) + Math.Sin(sinValue))/3),0);
            enemyObjects.transform.position = v;
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
            weaponFieldEmpty.SetActive(true);
            weaponField.ActivateInputField();
        }
	}
}