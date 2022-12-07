using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossFadeScript : MonoBehaviour {

    //Variables
    public enum FadeStates { None, In, Out }
    public FadeStates State = FadeStates.Out;
    private float a = 1;
    private Image BlackBox;
    // Start is called before the first frame update
    void Start() {
        BlackBox = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        if(State == FadeStates.Out) {
            a = Mathf.Lerp(a, -0.1f, 0.05f);
            if (a <= 0f) {
                State = FadeStates.None;
            }
        } else if(State == FadeStates.In) {
            a = Mathf.Lerp(a, 1.1f, 0.05f);
            if(a >= 1f) {
                State = FadeStates.None;
            }
        }
        BlackBox.color = new Color(0,0,0, a);
    }
}
