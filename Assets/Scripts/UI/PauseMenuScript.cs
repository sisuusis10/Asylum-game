using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PauseMenuScript : MonoBehaviour {

    //Variables
    public static PauseMenuScript pause;

    //Game
    public TextMeshProUGUI TMproText;
    public bool IsPaused = false;

    //Animation
    private bool dir = false;
    private float a = 0, l = 0.05f;

    // Start is called before the first frame update
    public void Awake() {
        pause = this;
        TMproText = this.GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        Color c = new Color(1,1,1,a);
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetPauseState(!IsPaused);
        }
        if(IsPaused && !dir) {
            a = Mathf.Lerp(a, 1.5f, l);
            if(a >= 1.4f) { dir = true; }
        } else {
            a = Mathf.Lerp(a, -0.5f, l);
            if (a <= -0.4f) { dir = false; }
        }
        TMproText.color = c;
    }

    //Set Pause state
    public void SetPauseState(bool state) {
        IsPaused = state;
        TMproText.text = (state) ? "-PAUSED-" : "";
        Time.timeScale = (state) ? 0f : 1f;
        PSXEffects.psx.subtractFade = (state) ? 3 : 0;
        PSXEffects.psx.UpdateProperties();
        GameManagerScript.game.indicator.SetVisibility(!state);
        SoundSource.s.IsPaused = state;
        a = 0;
        dir = false;
        //Mouse
        Cursor.lockState = (state) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;

    }

}
