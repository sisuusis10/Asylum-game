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
    public int Max_SlotCount = 4;
    private InventoryItemSlotScript[] ItemSlots;
    public int CurrentIndex = -1;
    // Start is called before the first frame update
    void Start() {
        Bg = this.GetComponent<Image>();

        ItemSlots = new InventoryItemSlotScript[Max_SlotCount]; //Set Item slot array size
        int pos_offset = 80; //Add to offset position for every slot
        for (int i = 0; i < Max_SlotCount; i++) { //Loop and create all item slots
            GameObject instance = Instantiate(ItemSlot_Prefab); //Instanciate prefab
            RectTransform _rect = instance.GetComponent<RectTransform>();
            _rect.SetParent(Bg.rectTransform);
            _rect.localPosition = new Vector2(Bg.rectTransform.localPosition.x, pos_offset);
            ItemSlots[i] = instance.GetComponent<InventoryItemSlotScript>(); //Save Instance to array
            pos_offset -= 80; //Change Slot offset to avoid overlap
        }

    }

    // Update is called once per frame
    void Update() {
        if (Input.mouseScrollDelta.y != 0) {
            //Set to visible
            IsVisible = true;
            p = -1f;
            d = 3f;
            //Scroll
            CurrentIndex -= (int)Input.mouseScrollDelta.y;
            if(CurrentIndex >= Max_SlotCount || CurrentIndex < 0) {
                CurrentIndex = (CurrentIndex < 0) ? Max_SlotCount : 0;
            }
            //Update visuals
            for(int i = 0; i < Max_SlotCount; i++) {
                bool r = (i == CurrentIndex);
                ItemSlots[i].SetState(r);
            }
        } else if(d > 0 && !PauseMenuScript.pause.IsPaused) {
            d -= Time.deltaTime;
        } else {
            //Set to invisible
            IsVisible = false;
            p = -2f;
        }
        Bg.rectTransform.pivot = new Vector2(p, 0.5f);
    }

}
