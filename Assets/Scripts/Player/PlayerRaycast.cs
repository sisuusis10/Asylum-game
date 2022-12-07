using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour {

    //Variables
    private Camera cam;
    private RaycastHit Hit;
    private float Distance = 2.5f;
    // Start is called before the first frame update
    void Start() {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        bool r = false;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out Hit, Distance)) {
            Trigger t = Hit.collider.gameObject.GetComponent<Trigger>();
            if(t && t.RequiresPlayerInput) {
                r = true;
                if(Input.GetKeyDown(KeyCode.E)) { t.ActivateTrigger(); }
            } else {
                r = false;
            }
        }
        GameManagerScript.game.indicator.SetIndicatorState(r);
    }
}
