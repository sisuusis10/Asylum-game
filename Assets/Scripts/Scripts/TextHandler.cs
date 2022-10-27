using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Events;
using UnityEngine.Localization.Tables;

public class TextHandler : MonoBehaviour {

    public static TextHandler set;

    private TextMeshProUGUI TMText, TMSkip;
    private Image ImageBG;
    public GameObject SkipText, BackGround;
    private Color BG_DefaultColor;

    private string TextString, FinalText;
    private long ItemEntryID;
    private float TextDelay = 0.03f, NewTextDelay;
    private float VelocityThreshold = 2f;
    private bool TW_Active;

    private bool CanBeHidden = false;

    //Movement
    private float Move_positive, Move_negative;

    //Localization
    public LocalizedString LocalizedString;

    private void Awake() {
        set = this;
        TMText = this.GetComponent<TextMeshProUGUI>();
        TMSkip = SkipText.GetComponent<TextMeshProUGUI>();
        ImageBG = BackGround.GetComponent<Image>();
        BG_DefaultColor = ImageBG.color;

        NewText("", false);
    }
    private void FixedUpdate() {
        //Movement
        if(TMText.text != "") {
            //Check if Movement is in negative
            if (Input.GetAxisRaw("Horizontal") == -1f || Input.GetAxisRaw("Vertical") == -1f) {
                Move_negative -= 0.05f;
            }
            //Check if Movement is in positive
            if (Input.GetAxisRaw("Horizontal") == 1f || Input.GetAxisRaw("Vertical") == 1f) {
                Move_positive += 0.05f;
            }
        }
        //DEBUG     print("MovPos:"+Move_positive+" MovNeg:"+Move_negative+" P_Mov:h"+ Input.GetAxisRaw("Horizontal")+"v"+Input.GetAxisRaw("Vertical"));
            //Check if player is moving
            if (Move_positive > VelocityThreshold || Move_negative < -VelocityThreshold || Move_negative > VelocityThreshold) {
            NewText("", true); //Make Sure no Text is being displayed
            //Reset Movements
            Move_positive = 0f;
            Move_negative = 0f;
        }

        //Adjust Type Speed
        if (Input.GetKey(ControlsHandler.get.Skip)) {
            NewTextDelay = 0f; //Fast Delay
            //Close text box
            NewText("", true);
        } else {
            NewTextDelay = 0.07f;
        }
    }

    //Hide / re appear when pausing the game
    private void Update() {
        if(CanBeHidden)
            if (GameHandler.set.IsPaused) { //Hide text box
                TMSkip.color = new Color(0f, 0f, 0f, 0f);
                ImageBG.color = new Color(0f, 0f, 0f, 0f);
                TMText.text = "";
            } else if (ImageBG.color == new Color(0f,0f,0f,0f) && !GameHandler.set.IsPaused) { //Re show text box
                TMSkip.color = Color.white;
                ImageBG.color = BG_DefaultColor;
                StartCoroutine(SetLocalizedText(TextString));
            }
    }

    public void NewText(string txt, bool ForceOverride, long itm_id = 0) {
        if(!TW_Active || ForceOverride) {
            StopAllCoroutines(); //Make sure no coroutines are active.
            TextString = txt; //Set Text
            ItemEntryID = itm_id; //set item id
        //    print(ItemEntryID);
            SetText();
            //StartCoroutine(TypeWriter()); //Start Coroutine
        }
    }

    private void SetText() {
        if (TextString == "") {
            TMSkip.color = new Color(0f, 0f, 0f, 0f);
            ImageBG.color = new Color(0f,0f,0f,0f);
            //Set text
            FinalText = TextString;
            TMText.text = FinalText;
            CanBeHidden = false;
        } else {
            TMSkip.color = Color.white;
            ImageBG.color = BG_DefaultColor;
            //Set localized text
            StartCoroutine(SetLocalizedText(TextString));
        }
        //TypeWriter Effect
        // FinalText = TextString;
        // TMText.text = FinalText;

        TW_Active = true;
        
        TW_Active = false;
    }

    //pain and agony |
    //               v
    private IEnumerator SetLocalizedText(string entry) {
        CanBeHidden = true;
        //Get entry ID
        long _entryID;
        //Try prase
        bool validID = long.TryParse(entry, out _entryID);
        //Check if ID is valid
        if (validID) {
            if (ItemEntryID != 0) {

                //Get interaction type
                var itm_op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("ItemTable", ItemEntryID);
                if (itm_op.IsDone) { // wait for operation to finish before executing rest of code

                    //Create Index
                    List<object> Index = new List<object>();

                    Index.Add(itm_op.Result);

                    //Set Text
                    var op2 = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("ObjectInfoTable", _entryID, Index);
                    if (op2.IsDone) {
                        FinalText = op2.Result;
                        TMText.text = FinalText; //Set text
                        StopCoroutine(SetLocalizedText(entry)); //End coroutine
                    } else {
                        op2.Completed += (o) => TMText.text = o.Result; //Set text
                        StopCoroutine(SetLocalizedText(entry)); //End coroutine
                    }

                }
            } else { //no item id

                //Set Text
                var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("ObjectInfoTable", _entryID);

                //Wait until operation is done before setting text
                yield return op.IsDone;
                if (op.IsDone) {
                    FinalText = op.Result;
                    TMText.text = FinalText; //Set text
                    StopCoroutine(SetLocalizedText(entry)); //End coroutine
                } else {
                    op.Completed += (o) => TMText.text = o.Result; //Set text
                    StopCoroutine(SetLocalizedText(entry)); //End coroutine
                }
            }

        } else {
            TMText.text = entry + "<br>(ERROR! no localization found! please report this issue)"; //Set text
            StopCoroutine(SetLocalizedText(entry)); //End coroutine
        }
    }

    //old
    private IEnumerator TypeWriter() {
        if(TextString == "") {
            SkipText.SetActive(false);
            BackGround.SetActive(false);
        } else {
            SkipText.SetActive(true);
            BackGround.SetActive(true);
        }
        for(int i = 0; i <= TextString.Length; i++) {
            //char array
            char[] c = TextString.ToCharArray();
            //Adjust Speed
            if (NewTextDelay == 0f) {
                TextDelay = NewTextDelay;
            } else if(i > 0 && i < c.Length-1) {
                 if (c[i] == ' ') {
                    TextDelay = 0f;
                } else if (c[i] == '.') {
                    TextDelay = 0.2f;
                } else if (c[i] == ',') {
                    TextDelay = 0.1f;
                } else {
                    TextDelay = NewTextDelay;
                }
            }

            //TypeWriter Effect
            FinalText = TextString.Substring(0, i);
            TMText.text = FinalText;
            TW_Active = true;
            yield return new WaitForSeconds(TextDelay);
        }
        TW_Active = false;
    }
}

//This is probably the script with the most amount of redundant code in the entire game god damn.
