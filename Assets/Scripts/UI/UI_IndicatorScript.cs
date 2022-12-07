using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_IndicatorScript : MonoBehaviour {

    //Variables
    private Image Image_Indicator;
    public TextMeshProUGUI InteractText;
    private Vector2 Size, SizeOriginal, SizeTarget;
    public float SizeMultiplier = 1.5f;
    //Color
    private float TextColor_a = 0f, TextColor_Target_a = 0f;
    // Start is called before the first frame update
    void Start() {
        Image_Indicator = this.GetComponent<Image>();
        SizeOriginal = this.transform.localScale;
        Size = SizeOriginal;
    }

    // Update is called once per frame
    void Update() {
        Size = Vector2.Lerp(Size, SizeTarget, 0.1f);
        TextColor_a = (!PauseMenuScript.pause.IsPaused) ? Mathf.Lerp(TextColor_a, TextColor_Target_a, 0.1f) : 0f;
        this.transform.localScale = Size;
        InteractText.color = new Color(1, 1, 1, TextColor_a);
    }

    public void SetIndicatorState(bool b) {
        SizeTarget = (!b) ? SizeOriginal : SizeOriginal * SizeMultiplier;
        TextColor_Target_a = (!b) ? -1f : 1f;
    }

    public void SetVisibility(bool b) {
        float r = (b) ? 1f : 0f;
        Color c = new Color(1, 1, 1, r);
        Image_Indicator.color = c;
    }

}
