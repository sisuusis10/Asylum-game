using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextBoxScript : MonoBehaviour {

    //Variables
    public bool IsActive = false;
    private Image BackGround;
    public TextMeshProUGUI Text;
    private float t;

    // Start is called before the first frame update
    void Start() {
        BackGround = this.GetComponent<Image>();
        SetState("");
    }

    private void Update() {
        if(IsActive) {
            t += PlayerController.p.MovementVector.magnitude;
            if(t > 20f) {
                SetState("");
                t = 0f;
            }
        }
    }

    public void SetState(string text) {
        Text.text = text;
        if (text == "") {
            BackGround.color = new Color(0,0,0,0);
            IsActive = false;
        } else {
            BackGround.color = new Color(0, 0, 0, 0.5f);
            IsActive = true;
        }
    }
}
