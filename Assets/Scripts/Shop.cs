using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    private bool subMenuOpen = false;
    public GameObject healthMenu;
    public GameObject weaponsMenu;
    public GameObject itemsMenu;
    public GameObject shelfText;
    public TextMeshProUGUI backText;
    public TextMeshProUGUI rughBalance;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weaponsText;
    public TextMeshProUGUI itemsText;

    public TextMeshProUGUI upgrade1;

    Dictionary<string, TextMeshProUGUI> possible = new Dictionary<string,TextMeshProUGUI>();
    string curInput = "";

    void Start()
    {
        possible.Add("Back", backText);
        possible.Add("Health", healthText);
        possible.Add("Weapons", weaponsText);
        possible.Add("Items", itemsText);
        //possible.Add("Upgrade 1", upgrade1);

        rughBalance.SetText($"{PlayerController.GetRugh()} R$");
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
                if (el.Key.Equals("Back")) {
                    if (subMenuOpen) {
                        healthMenu.SetActive(false);
                        weaponsMenu.SetActive(false);
                        itemsMenu.SetActive(false);
                        shelfText.SetActive(true);
                        subMenuOpen = false;
                    }
                    else {
                        SceneManager.LoadScene("Hub");
                    }
                }

                if (!subMenuOpen) {
                    if (el.Key.Equals("Health"))
                    {
                        healthMenu.SetActive(true);
                        shelfText.SetActive(false);
                        subMenuOpen = true;
                    }

                    if (el.Key.Equals("Weapons"))
                    {
                        weaponsMenu.SetActive(true);
                        shelfText.SetActive(false);
                        subMenuOpen = true;
                    }

                    if (el.Key.Equals("Items"))
                    {
                        itemsMenu.SetActive(true);
                        shelfText.SetActive(false);
                        subMenuOpen = true;
                    }
                } /*else {
                    if (el.Key.Equals("Upgrade 1")) {
                        if (PlayerController.GetRugh() >= 5) {
                            //things
                        } else {
                            upgrade1.SetText("Insufficient Balance");
                        }
                    }
                }*/
                curInput = "";
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
        return $"<#d4af37>{s.Substring(0, idx)}</color>{s.Substring(idx)}";
    }
}