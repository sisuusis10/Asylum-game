using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLighting_Script : Trigger_Collider {

    //Variables
    private Transform Cam;
    private bool Once = true;

    //Trigger Type
    public bool InFov_Trigger = true;

    //Lighthing
    private Color AmbientColor;
    public Color Area_AmbientLight = new Color(1, 0, 0, 0);

    public float LerpSpeed = 0f;

    private void Awake() {
        Cam = Camera.main.transform;
        if (Area_AmbientLight == new Color(1, 0, 0, 0)) {
            Area_AmbientLight = RenderSettings.ambientLight;
        }
    }

    private void FixedUpdate() {

    }
}
