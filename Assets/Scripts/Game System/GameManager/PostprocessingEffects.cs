using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PostprocessingEffects : MonoBehaviour {

    public static PostprocessingEffects effects;

    public PostProcessVolume Volume;
    public ChromaticAberration chrom;

    public float chrom_intensity = 0f;
    private float chrom_target_intensity = 0f;

    private void Awake() {
        effects = this;
    }

    private void Start() {
        chrom = ScriptableObject.CreateInstance<ChromaticAberration>();
        chrom.enabled.Override(true);
        chrom.fastMode.Override(false);
        chrom.intensity.Override(0f);

        Volume = PostProcessManager.instance.QuickVolume(3, 100f, chrom);
    }

    public void SetChromaticIntensity(float i) {
        chrom_target_intensity = i;
    }

    private void Update() {
            chrom_intensity = Mathf.Lerp(chrom_intensity, chrom_target_intensity, Time.deltaTime * 10);
        //Apply effects
        chrom.intensity.Override(chrom_intensity);
    }

}
