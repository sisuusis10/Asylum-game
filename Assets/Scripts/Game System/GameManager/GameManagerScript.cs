using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    //Variables
    public static GameManagerScript game;

    //Objects
    public UI_IndicatorScript indicator;

    // Start is called before the first frame update
    void Awake() {
        game = this;
        indicator = GameObject.FindObjectOfType<UI_IndicatorScript>();
    }
    private void Start() {
    }

}
