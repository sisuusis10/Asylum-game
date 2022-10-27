using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class IndicatorHandler : MonoBehaviour {
    public static IndicatorHandler set;
    private Image img;
    public Sprite Idle, Active;
    public TextMeshProUGUI Text;
    public Trigger TriggerScript;

    public bool IsActive;

    public string ControlsKey;
    public string InteractionType;
    public LocalizedString DisplayText;

    private void Awake() {
        set = this;
        img = this.GetComponent<Image>();
    }

    public void Indication(bool b) {
        IsActive = b;
        if (b) {
            img.overrideSprite = Active;

            //Localize
            LocalizedText(TriggerScript.Text());
            

            Text.gameObject.SetActive(true);
        } else {
            img.overrideSprite = Idle;
            Text.gameObject.SetActive(false);
        }
    }

    public void LocalizedText(string trigger_text) {
        ControlsKey = ControlsHandler.get.Interact.ToString().ToUpper();

        //Get interaction type
        var type = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("ItemTable", trigger_text);
        if (type.IsDone) { // wait for operation to finish before executing rest of code
                           //Get interaction type
            InteractionType = type.Result;

            //Create Index
            List<object> Index = new List<object>();
            Index.Add(ControlsKey);
            Index.Add(InteractionType);

            //Set Text
            //   var operation = LocalizationSettings.StringDatabase.GetLocalizedString("TextTable", "UI_IndicateItemInteraction");
            var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("TextTable", "UI_IndicateItemInteraction", Index);
            if (op.IsDone) {
                Text.text = op.Result;
            } else {
                op.Completed += (o) => Debug.Log(o.Result);
            }
        }
        if(ControlsKey == "ERROR" || InteractionType == "ERROR") {
            Text.text = "";
        }

    }

}
