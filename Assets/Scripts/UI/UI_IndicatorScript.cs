using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IndicatorScript : MonoBehaviour {

    //Variables
    private Image Image_Indicator;
    private Vector2 Size, SizeOriginal, SizeTarget;
    public float SizeMultiplier = 1.5f;
    // Start is called before the first frame update
    void Start() {
        Image_Indicator = this.GetComponent<Image>();
        SizeOriginal = this.transform.localScale;
        Size = SizeOriginal;
    }

    // Update is called once per frame
    void Update() {
        Size = Vector2.Lerp(Size, SizeTarget, 0.15f);
        this.transform.localScale = Size;
    }

    public void SetIndicatorState(bool b) {
        SizeTarget = (!b) ? SizeOriginal : SizeOriginal * SizeMultiplier;
    }

    public void SetVisibility(bool b) {
        float r = (b) ? 1f : 0f;
        Color c = new Color(1, 1, 1, r);
        Image_Indicator.color = c;
    }

}
