using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSign : MonoBehaviour {

    //Variables

    private Renderer _rend;

    //State
    public bool IsActive = true;
    
    public Material ActiveMat, InActiveMat;

    private void Start() {
        _rend = this.GetComponent<Renderer>();
    }

    private void UpdateState(bool state) {
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
