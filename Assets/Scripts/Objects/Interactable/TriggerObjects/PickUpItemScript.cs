using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemScript : Trigger {

    //Variables

    public override void ActivateTrigger() {
        this.transform.position = new Vector3(-99,-99,-99);
    }

}
