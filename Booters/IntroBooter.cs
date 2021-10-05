using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBooter : MonoBehaviour, IBooter {
    [SerializeField] private Canvas canvas;

    public void StartUp(Camera mainCamera) {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCamera;
    }

    public void Exit() {
    }

    public void GoToSelection() {
        Booter.instance.Change(0);
    }
}
