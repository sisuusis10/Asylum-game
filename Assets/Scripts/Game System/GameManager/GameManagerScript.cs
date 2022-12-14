using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    //Variables
    public static GameManagerScript game;

    //Objects
    public UI_IndicatorScript indicator;
    public TextBoxScript textbox;
    
    // Start is called before the first frame update
    void Awake() {
        game = this;
        indicator = GameObject.FindObjectOfType<UI_IndicatorScript>();
    }
    private void Start() {
    }

    public void SetScreenBlood(float time, float intensity) {

    }

    public void SetDeathScene() {
        SceneManager.LoadSceneAsync("DeathScene");
    }

}
