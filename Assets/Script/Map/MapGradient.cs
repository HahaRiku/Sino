using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MapGradient : MonoBehaviour {
    const int MaxGradientNum = 3;

    [System.Serializable]
    public struct GradientInfo {
        public float position;
        public Color color;
    }

    public Color leftColor;
    public GradientInfo[] gradients;
    public float gradientBias = 0.1f;
    private Material mat;

    /*private void Awake() {
        UpdateGradient();
    }*/

    private void Start() {
        mat = GetComponent<Image>().material;
        mat.SetColorArray("_GradientColors", new Color[] { Color.white, Color.white, Color.white, Color.white });
    }

    private void Update() {
        UpdateGradient();
    }

    void UpdateGradient() {
        if(mat!=null) {
            mat.SetFloat("_GradientCount", gradients.Length);
            mat.SetFloat("bias", gradientBias);
            Color[] colors = new Color[4];
            float[] positions = new float[gradients.Length];
            colors[0] = leftColor;
            for(int i = 0; i < gradients.Length; i++) {
                colors[i + 1] = gradients[i].color;
                positions[i] = gradients[i].position;
            }
            mat.SetColorArray("_GradientColors", colors);
            if(gradients.Length != 0) {
                mat.SetFloatArray("_GradientPositions", positions);
            }
        }
    }
}
