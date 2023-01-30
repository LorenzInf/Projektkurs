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
    private bool readying;
    private bool postAttack;
    private int fightTextOpacity = 99;
    private string[] allWords;
    private static string currWord;
    private char[] currWordArr;
    private string currDisplayedWord;
    private char[] currDisplayedWordArr;
    private string currInput;
    private int letterCount = 0;
    private int errors;
    private int attackSuccess; //0 = Miss, 1 = Barely, 2 = Okay, 3 = Good, 4 = Great, 5 = Perfect
    private WeaponController currWeapon;

    private int enemyLevel;
    private int enemyHealth;
    private int enemyMaxHealth;
    private int enemyDmg;

    public static bool _isBossFight;

    public GameObject oger;
    public GameObject fluffy;
    public GameObject enemyObjects;
    private string enemyType;
    
    public Image healthBarPlayer;
    public Image healthBarEnemy;
	public TMP_InputField weaponField;
    public GameObject weaponFieldEmpty;
    public GameObject headerTextEmpty;
	public TextMeshProUGUI text;
    public TextMeshProUGUI headerText;
    public Image timerBar;
    public TextAsset textAsset;
	public TextMeshProUGUI playerHealthNr;
	public TextMeshProUGUI enemyHealthNr;
	public GameObject bossRoom;
    
    void Start() {
        //Set Enemy stats
        enemyLevel = (int) (StatController._level * (_isBossFight ? 1 : 1.5));
        if(_isBossFight) {
			bossRoom.SetActive(true);
            enemyHealth = 50 + enemyLevel * 4;
            enemyMaxHealth = 50 + enemyLevel * 4;
        } else {
            enemyHealth = 10 + enemyLevel * 2;
            enemyMaxHealth = 10 + enemyLevel * 2;
        }
        enemyDmg = 10 + enemyLevel;

        //Load words
        allWords = textAsset.text.Split("\n");

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
		enemyHealthNr.text = ((int) enemyHealth).ToString();

        //Show intro text for 2 seconds
		timer = 1.5f;
		fightText = true;
        if(_isBossFight)
		    text.SetText("<#8B0000>Boss Fight!!!");
        else
            text.SetText("<#8B0000>Fight!");

        //Set player health bar
        healthBarPlayer.fillAmount = (float) (PlayerController.GetHealth() / PlayerController.GetMaxHealth());
		playerHealthNr.text = ((int) PlayerController.GetHealth()).ToString();
    }

    private void EndFight() {
        if(_isBossFight) {
            StatController.enemiesKilled++;
            LevelController.EndRun();
        } else {
            StatController.enemiesKilled++;
            (GameObject.Find("Main Camera").GetComponent("LevelController") as LevelController).EndFight();
            SceneManager.UnloadSceneAsync("Fight");
        }
    }
    
    //Called when weaponField is deselected
    public void SelectWeapon() {
        WeaponController weapon = (GameObject.Find("Player").GetComponent("PlayerController") as PlayerController).GetWeapon(weaponField.text);
        currWeapon = weapon;
        if(weapon != null) {
            (GameObject.Find("Player").GetComponent("PlayerController") as PlayerController).SetLastWeapon(weapon);
            //Set up attack
            attacking = true; //Make attacking true
            readying = true; //Make readying true
            weaponFieldEmpty.SetActive(false); //Remove the text field
            headerTextEmpty.SetActive(true); //Show the header
            currWord = allWords[(int) UnityEngine.Random.Range(0.0f, (float) allWords.Length)]; //Get a random word to type
            headerText.text = "<#BE271A>Get ready...";
            timer = 2f;
        } else if (weaponField.text.Contains("define") || weaponField.text.Contains("definition")) {
	    	if (currWord != null) {
				Application.OpenURL($"https://www.google.com/search?q=define+{currWord}");
				weaponField.text = "";
	    	} else {
				weaponField.text = "No word to define yet";
			}
	} else {
            weaponField.text = "You don't have that weapon";
        }
    }

    private void StartAttack() {
        errors = 0;
        currInput = "";
        currWordArr = currWord.ToCharArray();
        currDisplayedWord = $"<#1DF61B><#FFFFFF>{currWord}";
        text.SetText(currDisplayedWord); //Make the word to type visible
        letterCount = 0;
        headerText.text = "<#BE271A>Type!"; //Tell the player to type
        readying = false; //No longer readying
        timer = timerMax = 0.75f + (float) currWord.Length * 0.2f; //Set timer appropriately
        timerBar.color = new Color(105f / 255f, 229f / 255f, 51f / 255f, 1f);
    }

    private void ValidateAttack() {
        timer = 2;
        postAttack = true;
        attacking = false;
            int levenshteinDist = TypingController.Levenshtein(currInput,currWord);
            if (levenshteinDist == 0) {
                if (timerBar.fillAmount > 0.5) {
                    headerText.text = "<#53F855>Perfect!! / +20% DMG";
                    enemyHealth -= (int) (currWeapon.Use() * 1.2);
                    attackSuccess = 5;
                } else if (timerBar.fillAmount > 0.25) {
                    headerText.text = "<#8EF853>Great!";
                    enemyHealth -= (int) (currWeapon.Use());
                    attackSuccess = 4;
                } else {
                    headerText.text = "<#D3F853>Good";
                    enemyHealth -= (int) (currWeapon.Use());
                    attackSuccess = 3;
                }
            } else if ((float) levenshteinDist / (float) currWord.Length < 0.25f) {
                if (timerBar.fillAmount > 0.5) {
                    headerText.text = "<#D3F853>Good";
                    enemyHealth -= (int) (currWeapon.Use());
                    attackSuccess = 3;
                } else if (timerBar.fillAmount > 0.25) {
                    headerText.text = "<#F8E153>Okay / -10% Damage";
                    enemyHealth -= (int) (currWeapon.Use() * 0.9);
                    attackSuccess = 2;
                } else {
                    headerText.text = "<#F86353>Barely.. / -25% Damage";
                    enemyHealth -= (int) (currWeapon.Use() * 0.75);
                    attackSuccess = 1;
                }
            } else if ((float) levenshteinDist / (float) currWord.Length < 0.5f) {
                if (timerBar.fillAmount > 0.5) {
                    headerText.text = "<#F8E153>Okay / -10% Damage";
                    enemyHealth -= (int) (currWeapon.Use() * 0.9);
                    attackSuccess = 2;
                } else if (timerBar.fillAmount > 0.25) {
                    headerText.text = "<#F86353>Barely.. / -25% Damage";
                    enemyHealth -= (int) (currWeapon.Use() * 0.75);
                    attackSuccess = 1;
                } else {
                    headerText.text = "<#CD2626>Miss...";
                    attackSuccess = 0;
                }
            } else if ((float) levenshteinDist / (float) currWord.Length < 0.75f) {
                headerText.text = "<#CD2626>Miss...";
                attackSuccess = 0;
            } else {
                headerText.text = "<#CD2626>Miss...";
                attackSuccess = 0;
            }
		if (enemyHealth < 0) enemyHealth = 0;
        healthBarEnemy.fillAmount = (float) enemyHealth / enemyMaxHealth;
		enemyHealthNr.text = ((int) enemyHealth).ToString();
        StatController.wordsTyped++;
    }

    private void EnemyAttack(){
		bool fightOver = false;
		double attackModifier = 1;
        if(enemyHealth > 0) {
            weaponFieldEmpty.SetActive(true);
            weaponField.ActivateInputField();
        } else {
			fightOver = true; 
            EndFight();
        }
        headerTextEmpty.SetActive(false);
        timerBar.fillAmount = 0.0f;
        text.SetText("<#FFFFFF00>Placeholder");
        if (attackSuccess == 5) {
            attackModifier = 0;
        } else if (attackSuccess == 4) {
            attackModifier = 0.25;
        } else if (attackSuccess == 3) {
            attackModifier = 0.5;
        } else if (attackSuccess == 2) {
            attackModifier = 0.75;
        }
		if (fightOver) attackModifier = 0;
        (GameObject.Find("Player").GetComponent("PlayerController") as PlayerController).TakeDamage(enemyDmg * attackModifier);
		if (PlayerController._health < 0) PlayerController._health = 0;
        healthBarPlayer.fillAmount = (float) (PlayerController.GetHealth() / PlayerController.GetMaxHealth());
		playerHealthNr.text = ((int) PlayerController.GetHealth()).ToString();
        //TODO Enemy attack animation?
        if(PlayerController.GetHealth() <= 0) {
            StatController.lastRunSuccessful = false;
			PlayerController._health = PlayerController._maxHealth;
            StatController._tempRugh = 0;
			PlayerController.MovementLocked(false);
            SceneManager.LoadScene("ResultScreen");
        }
    }

    void Update() {
        //Count down timer
        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            if (postAttack) {
                postAttack = false;
                EnemyAttack();
            }
            if (fightText)
		        FightText();
            if (attacking && readying) {
                StartAttack();
            } else if (attacking && !readying) {
                ValidateAttack();
            }
        }

        //Count up sinValue
        sinValue += Time.deltaTime;

        if(enemyType.Equals("fluffy")) {
            //Move fluffy & health bar
            Vector2 v = new Vector2((float) ((Math.Sin(2*sinValue + 2) + Math.Sin(sinValue))/2),(float) ((Math.Cos(2.5*sinValue) + Math.Sin(sinValue))/1.5));
            enemyObjects.transform.position = v;
        } else if (enemyType.Equals("oger")) {
            //Move oger & health bar
            Vector2 v = new Vector2((float) ((Math.Sin(2*sinValue) + Math.Sin(sinValue))/3),0);
            enemyObjects.transform.position = v;
        }


        // -- PLAYER ATTACK --

        if (attacking) {
            headerTextEmpty.transform.position = new Vector2(0,(float) Math.Sin(4*sinValue)/7); //Wave Type! / ready text
        
            // --- After readying ---
            if (!readying) {
                //Set timerBar to percentage of timer while attacking
                timerBar.fillAmount = timer / timerMax;
                if(timer <= timerMax / 2) {
                    timerBar.color = Color.yellow;
                    if (timer <= timerMax / 4) {
                        timerBar.color = new Color(1f,0.25f,0.25f,1f);
                    }
                }
                
                //Read input
                foreach (char c in Input.inputString) {
                    if(Char.IsLetter(c)) {
                        currInput += c;
                        currDisplayedWordArr = currDisplayedWord.ToCharArray();
                        string newDisplayedWord = "";

                        //Append already typed
                        for(int i = 0; i < 9 /* < Initial colour code */ + letterCount + errors * 18 /* 2 Color codes with each error */; i++) {
                            newDisplayedWord += currDisplayedWordArr[i];
                        }

                        //If the letter that was just input is was correct
                        if(c.Equals(currWordArr[letterCount])) {
                            newDisplayedWord += c;
                        } else { //If it was incorrect
                            newDisplayedWord += $"<#EF1D0B>{c}<#1DF61B>";
                            errors++;
                            StatController.typos++;
                        }
                        letterCount++;
                        newDisplayedWord += "<#FFFFFF>";
                        for(int i = letterCount; i < currWordArr.Length; i++) {
                            newDisplayedWord += currWordArr[i];
                        }
                        currDisplayedWord = newDisplayedWord;
                        text.text = currDisplayedWord;
                        if(letterCount == currWord.Length) {
                            ValidateAttack();
                        }
                    }
                }
            }
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
