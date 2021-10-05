using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class LevelSelectionButton : MonoBehaviour {
    private int levelIndex;

    private TMP_Text text;

    private void Awake() {
        text = GetComponentInChildren<TMP_Text>();

        if (text == null) {
            Debug.LogErrorFormat("Button {0} has no text component.");
            return;
        }
    }

    public void SetLevel(int level) {
        levelIndex = level;
        text.text = (levelIndex + 1).ToString();
    }

    public void OnClick() {
        SelectionBooter.instance.Select(levelIndex);
    }
}
