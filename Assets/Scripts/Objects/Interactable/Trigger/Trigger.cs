using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    //Variables
    public bool IsActivated = false;
    public bool TriggerOnce = false;
    public virtual void ActivateTrigger() {
        if(TriggerOnce) {
            IsActivated = true;
        }
    }

}
