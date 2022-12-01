using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour {

    //Variables
    public bool IsVisible;
    public GameObject ItemSlot_Prefab;
    private Image Bg;
    private float d, p = -1f;
    // Start is called before the first frame update
    void Start() {
        Bg = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.mouseScrollDelta.y != 0) {
            IsVisible = true;
            p = 0.5f;
            d = 3f;
        } else if(d > 0) {
            d -= Time.deltaTime;
        } else {
            IsVisible = false;
            p = -1f;
        }
        Bg.rectTransform.pivot = new Vector2(p, 0.5f);
    }
}
