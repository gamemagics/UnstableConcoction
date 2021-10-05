using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBooter : MonoBehaviour, IBooter {
    public static SelectionBooter instance = null;

    private void Awake() {
        instance = this;
    }

    public void StartUp(Camera mainCamera) {
        Canvas canvas = GetComponentInChildren<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCamera;
    }

    public void Exit() {
    }

    public void Select(int level) {
        Booter.levelIndex = level;
        Booter.instance.Change(1);
    }
}
