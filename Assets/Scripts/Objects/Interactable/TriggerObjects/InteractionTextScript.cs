using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTextScript : Trigger {

    //Variables
    public string Text = "place holder";
    public override void ActivateTrigger() {
        GameManagerScript.game.textbox.SetState(Text);
    }

}
