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
    private string[] allWords;

    public static bool _isBossFight;

    public GameObject oger;
    public GameObject fluffy;
    
    public Image healthBarPlayer;
    public Image healthBarEnemy;
	public TMP_InputField weaponField;
    public GameObject weaponFieldEmpty;
	public TextMeshProUGUI text;
    
    void Start() {
        //Load words
        Resources.Load("Assets/Text/words.txt");
        allWords = System.IO.File.ReadAllLines("Assets/Text/words.txt");

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
    }

    private void EndFight() {
        PlayerController.MovementLocked(false);
        SceneManager.UnloadSceneAsync("Fight");
    }
    
    //Called when weaponField is deselected
    public void SelectWeapon() {
        WeaponController weapon = (GameObject.Find("Player").GetComponent("PlayerController") as PlayerController).GetWeapon(weaponField.text);
        if(weapon != null) {
            Debug.Log($"Weapon: {weapon.GetName()}");
        } else {
            Debug.Log("Didn't find a weapon");
        }
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
            weaponFieldEmpty.SetActive(true);
            weaponField.ActivateInputField();
        }
	}
}