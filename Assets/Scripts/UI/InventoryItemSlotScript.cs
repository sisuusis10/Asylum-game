using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlotScript : MonoBehaviour {

    public Image ItemSlot;
    public Image ItemDisplay;

    public Sprite[] StateSprites;
    public bool IsSelected = false;

    private float pos_offset, pos_Highlight, lerp;

    private void Start() {
        ItemSlot = this.GetComponent<Image>();
        ItemDisplay.gameObject.SetActive(false
            );
    }
    public void SetState(bool _selected) {
        IsSelected = _selected;
        ItemSlot.sprite = StateSprites[(_selected) ? 1 : 0];
    }

    public void SetVisibility(bool IsVisible, int _index) {
        pos_offset = (IsVisible) ? 1f : 0f;
        float l = 1f / _index+1;
    }

    public void Update() {
        Vector2 vec_pos = ItemSlot.rectTransform.position;

        //
        

        //Apply
       // ItemSlot.rectTransform.localPosition = vec_pos;
    }

}
