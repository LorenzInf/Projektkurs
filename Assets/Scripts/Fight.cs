using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fight : MonoBehaviour
{
    public GameObject oger;
    public GameObject fluffy;
    
    public Image healthBarPlayer;
    public Image healthBarEnemy;
    
    void Start() {
        PlayerController.MovementLocked(true);
        if (Random.Range(-1.0f, 1.0f) > 0.0) {
            oger.SetActive(true);
        } else {
            fluffy.SetActive(true);
        }
        
    }

    private void EndFight() {
        PlayerController.MovementLocked(false);
        SceneManager.UnloadSceneAsync("Fight");
    }
    
    void Update() {
        healthBarPlayer.fillAmount = (float) (PlayerController.GetHealth() / PlayerController.GetMaxHealth());
        
    }
}