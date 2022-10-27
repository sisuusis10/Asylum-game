using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileTexture : MonoBehaviour {

    private Renderer Rend;

    public bool DisableAutoCalculations = false;
    private Vector2 CalculatedScale;
    public Vector2 DefaultTileScale;

    private Vector3 PrevObjScale;
    private Vector2 PrevDefaultTileScale;

    private Material Mat_NewInstance;

    public Vector2 Animate_Offset;

    // Start is called before the first frame update
    void Start() {
        Rend = this.GetComponent<Renderer>();
        if(DefaultTileScale == new Vector2(0f,0f) && PrevDefaultTileScale == new Vector2(0f, 0f) && CalculatedScale == new Vector2(0f, 0f) && PrevObjScale == new Vector3(0f, 0f, 0f)) {
            DefaultTileScale = Rend.sharedMaterial.mainTextureScale;
        }

        UpdateTile();
    }

    private void OnDrawGizmos() {
        if (Application.isEditor) {
            if(PrevObjScale != this.transform.localScale) {
                PrevObjScale = this.transform.localScale;
                UpdateTile();
            } else if(DefaultTileScale != PrevDefaultTileScale) {
                PrevDefaultTileScale = DefaultTileScale;
                UpdateTile();
            }
        }
    }

    private void FixedUpdate() {
        if(Animate_Offset != Vector2.zero && Application.isPlaying) {
            Rend.material.mainTextureOffset = Animate_Offset * Time.time;
        }
    }

    private void UpdateTile() {
        if(!DisableAutoCalculations) {
            CalculatedScale.x = DefaultTileScale.x * (this.transform.localScale.x);
            CalculatedScale.y = DefaultTileScale.y * (this.transform.localScale.z);
        } else {
            CalculatedScale = DefaultTileScale;
        }
        try {
            Mat_NewInstance = new Material(Rend.sharedMaterial);
            Rend.material = Mat_NewInstance;
        Mat_NewInstance.mainTextureScale = CalculatedScale;
        } catch { print("Error! unable to set mat Instance to renderer's shared material."); }
    }

}
