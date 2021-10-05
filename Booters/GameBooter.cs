using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBooter : MonoBehaviour, IBooter {
    public static GameBooter instance;

    [SerializeField] private Levels levels;

    public bool passed = false;

    private void Awake() {
        instance = this;
    }

    public void StartUp(Camera mainCamera) {
    }

    public void Exit() {
        if (passed) {
            passed = false;
            PlayerPrefs.SetInt("level", Mathf.Min(PlayerPrefs.GetInt("level") + 1, 999));
            PlayerPrefs.Save();
        }
    }
}
