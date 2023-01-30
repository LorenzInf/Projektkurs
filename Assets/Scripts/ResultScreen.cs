using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultScreen : MonoBehaviour
{
    public TextMeshProUGUI successText;

    public TextMeshProUGUI wordsTypedText;
    public TextMeshProUGUI typosText;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI rughEarnedText;
    public TextMeshProUGUI levelText;

    public TextMeshProUGUI continueText;

    private bool successSet;
    private bool wordsSet;
    private bool typosSet;
    private bool enemiesSet;
    private bool rughSet;
    private bool levelSet;
    private bool numbersCorrected;
    private bool continueSet;

    private float wordsNumber;
    private float typosNumber;
    private float enemiesNumber;
    private float rughNumber;
    private float levelNumber;

    private float timer = 0.0f;
    private Dictionary<string, TextMeshProUGUI> possible = new Dictionary<string,TextMeshProUGUI>();
    string curInput = "";

    void Update() {
        if (timer < 7.0f) {
            timer += Time.deltaTime;
        } else if (!continueSet) {
            possible.Add("Continue",continueText);
            continueSet = true;
            continueText.gameObject.SetActive(true);
        }

        //After 1 second show success
        if (timer >= 1.0f && !successSet) {
            successSet = true;
            successText.gameObject.SetActive(true);
            if(StatController.lastRunSuccessful) {
                successText.text = "<#50CF30>Run Successful";
            } else {
                successText.text = "<#BA2B30>You Died";
            }
        }

        //After 3 seconds, begin to show stats
        if (timer >= 3.0f && !wordsSet) {
            wordsNumber = 999;
            wordsSet = true;
            wordsTypedText.gameObject.SetActive(true);
        }
        if (wordsSet && wordsNumber > StatController.wordsTyped + 1) {
            wordsNumber -= Time.deltaTime * (999 - StatController.wordsTyped)/3;
            wordsTypedText.text = $"Words Typed:<line-height=0>\n<align=right>{(int) wordsNumber}";
        }

        if (timer >= 3.5f && !typosSet) {
            typosNumber = 999;
            typosSet = true;
            typosText.gameObject.SetActive(true);
        }
        if (typosSet && typosNumber > StatController.typos + 1) {
            typosNumber -= Time.deltaTime * (999 - StatController.typos)/2.5f;
            typosText.text = $"Typos Made:<line-height=0>\n<align=right>{(int) typosNumber}";
        }

        if (timer >= 4.0f && !enemiesSet) {
            enemiesNumber = 999;
            enemiesSet = true;
            enemiesKilledText.gameObject.SetActive(true);
        }
        if (typosSet && enemiesNumber > StatController.enemiesKilled + 1) {
            enemiesNumber -= Time.deltaTime * (999 - StatController.enemiesKilled)/2f;
            enemiesKilledText.text = $"Enemies Killed:<line-height=0>\n<align=right>{(int) enemiesNumber}";
        }

        if(StatController.lastRunSuccessful) {
            if (timer >= 4.5f && !rughSet) {
                rughNumber = 999;
                rughSet = true;
                rughEarnedText.gameObject.SetActive(true);
            }
            if (rughSet && rughNumber > StatController._tempRugh + 1) {
                rughNumber -= Time.deltaTime * (999 - StatController._tempRugh)/1.5f;
                rughEarnedText.text = $"Rugh Earned:<line-height=0>\n<align=right>{(int) rughNumber}";
            }

            if (timer >= 5.0f && !levelSet) {
                levelNumber = 999;
                levelSet = true;
                levelText.gameObject.SetActive(true);
            }
            if (typosSet && levelNumber > StatController._level + 1) {
                levelNumber -= Time.deltaTime * (999 - StatController._level)/1f;
                levelText.text = $"Level:<line-height=0>\n<align=right>{(int) levelNumber}";
            }
        }

        //Failsafe, in case of lag
        if (timer >= 6.0f && !numbersCorrected) {
            numbersCorrected = true;
            wordsTypedText.text = $"Words Typed:<line-height=0>\n<align=right>{StatController.wordsTyped}";
            typosText.text = $"Typos Made:<line-height=0>\n<align=right>{StatController.typos}";
            enemiesKilledText.text = $"Enemies Killed:<line-height=0>\n<align=right>{StatController.enemiesKilled}";
            rughEarnedText.text = $"Rugh Earned:<line-height=0>\n<align=right>{StatController._tempRugh}";
            levelText.text = $"Level:<line-height=0>\n<align=right>{StatController._level}";
        }

        foreach (char c in Input.inputString) {
            curInput += c;
        }

        bool any = false;
        foreach (KeyValuePair<string, TextMeshProUGUI> el in possible) {
            if (el.Key.ToLower().Equals(curInput)) {
                if (el.Key.Equals("Continue")) {
                    StatController.Reset();
                    SceneManager.LoadScene("Hub");
                    curInput = "";
                }
            }
            if (el.Key.ToLower().StartsWith(curInput.ToLower())) {
                any = true;
                el.Value.text = AtIndex(el.Key, curInput.Length);
            }
        }
        if (!any) {
            curInput = "";
        }
    }

    private string AtIndex (string s, int idx) {
        return $"<#d4af37>{s.Substring(0, idx)}</color>{s.Substring(idx)}";
    }
}
