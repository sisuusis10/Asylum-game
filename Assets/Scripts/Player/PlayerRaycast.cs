using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour {

    //Variables
    public static PlayerRaycast uh;

    private Camera cam;
    private RaycastHit Hit;
    private float Distance = 2.5f;
    // Start is called before the first frame update
    void Start() {
        uh = this;
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

    //check if in fov
    public bool IsInFov(Transform Entity) {
        Vector3 dir = Entity.position - cam.transform.position;
        float angle = Vector3.Angle(dir, cam.transform.forward);
        //check if in fov
        if (angle <= cam.fieldOfView) {
            RaycastHit FovHit;
            if (Physics.Raycast(cam.transform.position, dir.normalized, out FovHit)) {
                if (FovHit.collider.gameObject == Entity.gameObject) {
                    print(Entity.name + " is in view.");
                    //PostProcessing effects
                    PostprocessingEffects.effects.SetChromaticIntensity(1f);

                    return true; //return true if object is detected
                }
            }
        }
        print(this.name + " is not in view.");
        PostprocessingEffects.effects.SetChromaticIntensity(0f);
        return false; //True is not returned, then return false
    }

}
