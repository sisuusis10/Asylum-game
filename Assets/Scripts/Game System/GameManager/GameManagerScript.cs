using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManagerScript : MonoBehaviour {

    //Variables
    public static GameManagerScript game;

    //Game
    public bool IsPaused = false;

    //Child GameObjects
    public TextMeshProUGUI PausedText;

    // Start is called before the first frame update
    void Awake() {
        game = this;
    }
    private void Start() {
        SetPauseState(IsPaused);
    }
    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            bool s = (IsPaused) ? false : true;
            SetPauseState(s);
        }
    }

    //Set Pause state
    public void SetPauseState(bool state) {
        IsPaused = state;
        PausedText.enabled = state;        
        Time.timeScale = (state) ? 1f : 0f;
    }

}
