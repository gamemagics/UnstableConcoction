using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    [SerializeField] private Levels levels;
    [SerializeField] private GameObject buttonPrefab;

    private void Awake() {
        if (!PlayerPrefs.HasKey("level")) {
            PlayerPrefs.SetInt("level", 0);
            PlayerPrefs.Save();
        }

        int max = PlayerPrefs.GetInt("level");
        for (int i = 0; i < levels.levelPrefabs.Length; ++i) {
            GameObject button = Instantiate(buttonPrefab);
            var selection = button.GetComponent<LevelSelectionButton>();

            if (i > max) {
                Button bt = button.GetComponent<Button>();
                bt.enabled = false;
            }
            
            if (selection == null) {
                Debug.LogError("Buttons need LevelSelectionButton.");
                return;
            }

            selection.SetLevel(i);
            var rectTransform = button.GetComponent<RectTransform>();
            rectTransform.SetParent(GetComponent<RectTransform>());
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
