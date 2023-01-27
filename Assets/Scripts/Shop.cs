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
	public TextMeshProUGUI upgrade2;
	public TextMeshProUGUI upgrade3;
    public TextMeshProUGUI hundredHealth1;
    public TextMeshProUGUI hundredHealth2;
    public TextMeshProUGUI hundredHealth3;
	public GameObject soldOut1;
	public GameObject soldOut2;
	public GameObject soldOut3;
	public GameObject locked1;
	public GameObject locked2;
	public int ownedHealthUps;

    Dictionary<string, TextMeshProUGUI> possible = new Dictionary<string,TextMeshProUGUI>();
    string curInput = "";

    void Start() {
        possible.Add("Back", backText);
        possible.Add("Health", healthText);
        possible.Add("Weapons", weaponsText);
        possible.Add("Items", itemsText);

        rughBalance.SetText($"{PlayerController.GetRugh()} R$");
		ownedHealthUps = (((int) PlayerController.GetMaxHealth() - 100) / 100);

		if (ownedHealthUps > 0) {
			upgrade1.SetText("<#808080>Upgrade");
            hundredHealth1.SetText("<#808080>+100 Max Health");
            soldOut1.SetActive(true);
            if(ownedHealthUps == 1) {
                upgrade3.SetText("<#808080>Upgrade");
                hundredHealth3.SetText("<#808080>+100 Max Health");
                locked2.SetActive(true);
                possible.Add("Upgrade",upgrade2);
            } else if (ownedHealthUps >= 2) {
                upgrade2.SetText("<#808080>Upgrade");
                hundredHealth2.SetText("<#808080>+100 Max Health");
                soldOut2.SetActive(true);
                if (ownedHealthUps == 3) {
                    upgrade3.SetText("<#808080>Upgrade");
                    hundredHealth3.SetText("<#808080>+100 Max Health");
                    soldOut3.SetActive(true);
                } else {
                    possible.Add("Upgrade",upgrade3);
                }
            }
		} else {
            upgrade2.SetText("<#808080>Upgrade");
            hundredHealth2.SetText("<#808080>+100 Max Health");
            upgrade3.SetText("<#808080>Upgrade");
            hundredHealth3.SetText("<#808080>+100 Max Health");
            locked1.SetActive(true);
            locked2.SetActive(true);
            possible.Add("Upgrade",upgrade1);
        }
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
                } else {
                    if (el.Key.Equals("Upgrade")) {
                        if (ownedHealthUps == 0) {
                            if (PlayerController.GetRugh() >= 5) {
                                PlayerController.AddRugh(-5);
                                rughBalance.SetText($"{PlayerController.GetRugh()} R$");
                                PlayerController.Purchasable("health",true);
                                upgrade1.SetText("<#808080>Upgrade");
                                hundredHealth1.SetText("<#808080>+100 Max Health");
                                soldOut1.SetActive(true);
                                ownedHealthUps = -1;
                            } else {
                                hundredHealth1.SetText("Insufficient Balance");
                            }
                        } else if (ownedHealthUps == 1) {
                            if (PlayerController.GetRugh() >= 10) {
                                PlayerController.AddRugh(-10);
                                rughBalance.SetText($"{PlayerController.GetRugh()} R$");
                                PlayerController.Purchasable("health",true);
                                upgrade2.SetText("<#808080>Upgrade");
                                hundredHealth2.SetText("<#808080>+100 Max Health");
                                soldOut2.SetActive(true);
                                ownedHealthUps = -1;
                            } else {
                                hundredHealth2.SetText("Insufficient Balance");
                            }
                        } else if (ownedHealthUps == 2) {
                            if (PlayerController.GetRugh() >= 15) {
                                PlayerController.AddRugh(-15);
                                rughBalance.SetText($"{PlayerController.GetRugh()} R$");
                                PlayerController.Purchasable("health",true);
                                upgrade2.SetText("<#808080>Upgrade");
                                hundredHealth2.SetText("<#808080>+100 Max Health");
                                soldOut2.SetActive(true);
                                ownedHealthUps = -1;
                            } else {
                                hundredHealth3.SetText("Insufficient Balance");
                            }
                        }
                    }
                }
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