using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_RayCaster : MonoBehaviour {

    public static Camera_RayCaster get;
    //Raycaster
    public RaycastHit Hit;
    public Trigger TriggerScript;
    public float Distance = 2f;
    //Field of view raycast
    public float Fov;
    //Pause Delay
    private bool IsActive = true;
    private float Timer, TimerMax = 0.1f;

    private void Awake() {
        get = this;
    }
    private void Start() {
       Fov = Camera.main.fieldOfView;
    }
    // Update is called once per frame
    void Update() {
        //Make sure player isnt locked
        if(!PlayerController.p.IsLocked) {
            //Raycast What player is looking at
            if (Physics.Raycast(this.transform.position, this.transform.forward, out Hit, Distance)) {
                if (Hit.collider.gameObject.GetComponent<Trigger>() && !Hit.collider.gameObject.GetComponent<Trigger>().Ignore) {
                    IndicatorHandler.set.TriggerScript = Hit.collider.gameObject.GetComponent<Trigger>();
                    IndicatorHandler.set.Indication(true);
                    if (Input.GetKeyDown(KeyCode.E) && IsActive) {
                        Hit.collider.gameObject.GetComponent<Trigger>().IsActive = true;
                    }
                } else {
                    IndicatorHandler.set.Indication(false);
                }
            } else {
                IndicatorHandler.set.Indication(false);
            }
        } else { //Is Paused
            IndicatorHandler.set.Indication(false);
            IsActive = false;
        }

        if(!IsActive && Timer < TimerMax) {
            Timer += Time.deltaTime;
        } else {
            IsActive = true;
            Timer = 0f;
        }
    }

}
