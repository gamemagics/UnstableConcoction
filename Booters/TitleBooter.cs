using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBooter : MonoBehaviour , IBooter{
    public void StartUp(Camera mainCamera) {
        Canvas canvas = GetComponentInChildren<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCamera;
    }

    public void Exit() {
    }
}
