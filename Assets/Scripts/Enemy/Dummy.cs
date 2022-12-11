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
        if (d < 20) {
            float a = Mathf.Abs(d / 5f) - 1f;
            SoundSource.s.SetLevels(a, true);
        } else {
            SoundSource.s.SetLevels(1f, false);
        }
        if(d < 25) {
            float r = (d < 20 && PlayerRaycast.uh.IsInFov(this.transform)) ? 1f : 0f;
            PostprocessingEffects.effects.SetChromaticIntensity(r);
        }
    }
}
