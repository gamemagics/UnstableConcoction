using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour {

    Light2D light2d;

    private void Awake() {
        light2d = GetComponent<Light2D>();
    }

    public void SetColor(Color color) {
        light2d.color = color;
    }

    public void TurnOff() {
        light2d.intensity /= 2.0f;
    }
}
