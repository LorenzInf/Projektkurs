using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Hub : MonoBehaviour
{
    public TextMeshProUGUI beginText;
    public TextMeshProUGUI shopText;
    public TextMeshProUGUI exitText;
    
    Dictionary<string, TextMeshProUGUI> possible = new Dictionary<string,TextMeshProUGUI>();
    string curInput = "";

    void Start () {
        possible.Add("Begin",beginText);
        possible.Add("Shop",shopText);
        possible.Add("Exit",exitText);
    }

    void Update () {
        foreach (char c in Input.inputString) {
            if (c == '\b') {
                curInput = curInput.Substring(0, curInput.Length - 1);
            } else {
                curInput += c;
            }
        }
        bool any = false;
        foreach (KeyValuePair<string, TextMeshProUGUI> el in possible) {
            if (el.Key.ToLower().Equals(curInput)) {
                if (el.Key.Equals("Exit")) {
                    Application.Quit();
                    Debug.Log("The application was quit");
                    curInput = "";
                }

                if (el.Key.Equals("Begin")) {
                    SceneManager.LoadScene("Level");
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

    string AtIndex (string s, int idx) {
        return $"<color=\"yellow\">{s.Substring(0, idx)}<color=\"black\">{s.Substring(idx)}";
    }
}