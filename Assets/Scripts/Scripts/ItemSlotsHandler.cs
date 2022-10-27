using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotsHandler : MonoBehaviour {

    public GameObject SlotPrefab;
    [SerializeField]
    private Image[] ItemSlots;

    public int Max_SlotCount = 4;

    public Sprite Sprite_Default, Sprite_Selected;

    private Animator SlotsAnimator;

    public bool Slots_Show;

    private int Current_Slot;

    private void Start() {

        SlotsAnimator = GetComponent<Animator>(); //set animator

        ItemSlots = new Image[Max_SlotCount]; //Set Item slot array size

        int pos_offset = 0; //Add to offset position for every slot
        for (int i = 0; i < Max_SlotCount; i++) { //Loop and create all item slots
            GameObject instance = Instantiate(SlotPrefab); //Instanciate prefab

            instance.transform.SetParent(this.transform); //set insance's parent to this
            instance.transform.position = this.transform.position + new Vector3(0, pos_offset, 0); //add offset

            ItemSlots[i] = instance.GetComponent<Image>(); //Save Instance to array
            pos_offset -= 65; //Change Slot offset to avoid overlap
        }
    }

    private void Update() {
        //Condition for setting Visibility
        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0f) {
            Slots_Show = true; //set true
            StopAllCoroutines();
            StartCoroutine(Hide_Slots()); //Start Hide Counter
        }
        
        //set Slot visibility
        SlotsAnimator.SetBool("Show", Slots_Show); //set animator bool to Slots_Show state
        
        //Change Item
        if(SlotsAnimator.GetCurrentAnimatorStateInfo(0).IsName("ItemSlots_Open")) { //check current animation
            if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f) { //add 1 to current slot
                Current_Slot--;
            } else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f) { //Remove 1 from current slot
                Current_Slot++;
            }
        } else {
            StopAllCoroutines(); //Stop Coroutines when Slots are Hidden 
        }
        Current_Slot = Mathf.Clamp(Current_Slot, 0, Max_SlotCount-1); //Clamp Current Slot

        //Set slot selected or Unselected
        for(int i = 0; i < ItemSlots.Length; i++) {
            if(i == Current_Slot) {
                ItemSlots[i].sprite = Sprite_Selected;
            } else {
                ItemSlots[i].sprite = Sprite_Default;
            }
        }
    }

    private IEnumerator Hide_Slots() {
        yield return new WaitForSeconds(2f); //wait for 2 seconds
        Slots_Show = false; //Set Slots_Show to false
    }
}
