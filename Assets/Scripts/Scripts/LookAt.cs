using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    //Variables
    public GameObject Target;
    private void Start() {
        if(Target == null) {
            Target = PlayerController.p.gameObject;
        }
    }
    // Update is called once per frame
    void Update() {
        this.transform.LookAt(Target.transform);
    }
}
