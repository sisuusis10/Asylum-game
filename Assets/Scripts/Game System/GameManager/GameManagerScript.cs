using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    //Variables
    public static GameManagerScript game;

    //Objects
    public UI_IndicatorScript indicator;
    public TextBoxScript textbox;
    public Image BloodOverlay;
    private float BloodEffect_Time, TargetIntensity, Intensity;
    
    // Start is called before the first frame update
    void Awake() {
        game = this;
        indicator = GameObject.FindObjectOfType<UI_IndicatorScript>();
    }

    private void Update() {
        if(BloodEffect_Time > 0f) {
            BloodEffect_Time -= Time.deltaTime;
        } else if(TargetIntensity > 0f) {
            TargetIntensity = Mathf.Lerp(TargetIntensity, -0.1f, Time.deltaTime / 5f);
        }
        Intensity = Mathf.Lerp(Intensity, TargetIntensity, Time.deltaTime);
        BloodOverlay.color = new Color(1,1,1, Intensity);
    }

    public void SetScreenBlood(float time, float intensity) {
        BloodEffect_Time = time;
        if(TargetIntensity <= 1f) {
            TargetIntensity += intensity;
        }
    }

    public void SetDeathScene() {
        SceneManager.LoadSceneAsync("DeathScene");
    }

}
