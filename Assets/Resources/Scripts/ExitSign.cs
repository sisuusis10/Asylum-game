using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSign : MonoBehaviour {

    //Variables
    public Trigger t_Active;
    public Trigger t_Flicker;

    private Renderer _rend;

    //State
    public bool IsActive = true;
    
    public Material ActiveMat, InActiveMat;

    private void Start() {
        _rend = this.GetComponent<Renderer>();

        IsActive = t_Active;
        UpdateVisuals(IsActive);
    }


    public void SetState(bool active, bool flicker) {
        IsActive = active;
        t_Active.IsActive = active;
        t_Flicker.IsActive = flicker;

        UpdateVisuals(IsActive);
    }

    private void UpdateVisuals(bool state) {
        switch (state) {
            case true:
                _rend.material = ActiveMat;
                break;
            case false:
                _rend.material = InActiveMat;
                break;
        }
    }

}
