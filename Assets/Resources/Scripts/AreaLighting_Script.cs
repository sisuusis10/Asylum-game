using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLighting_Script : MonoBehaviour {

    //Variables
    private Transform Cam;
    private Trigger_Collider t;
    private bool Once = true;

    //Trigger Type
    public bool InFov_Trigger = true;

    //Lighthing
    public Color Area_AmbientLight = new Color(1,0,0,0);
    public float LerpSpeed = 0f;

    private void Awake() {
        t = this.GetComponent<Trigger_Collider>(); //Set t to trigger collider
        Cam = Camera.main.transform;
        if (Area_AmbientLight == new Color(1, 0, 0, 0)) {
            Area_AmbientLight = RenderSettings.ambientLight;
        }
    }

    private void FixedUpdate() {
        if(t.IsActive) {
            LevelGlobals.get.NewAreaLight(Area_AmbientLight, LerpSpeed);
            PlayerController.p.IsInAreaLight = true;
            Once = false;
        } else if(!Once) {
            PlayerController.p.IsInAreaLight = false;
            Once = true;
        }
    }
}
