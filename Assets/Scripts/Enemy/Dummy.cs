using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {

    public Transform Player;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        float d = Vector3.Distance(Player.position, transform.position);
        if (d < 10) {
            float a = 1f - Mathf.Abs(d / 10f);
            SoundSource.s.SetLevels(d, true);
            print(a);
        } else {
            SoundSource.s.SetLevels(0f, false);
        }
    }
}
