using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class SpriteController : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    [System.Serializable]
    public struct SpriteInfo
    {
        public string name;
        public Sprite sprite;
    }

    public SpriteInfo[] sprites;

    private void Start()
    {
        dialogueRunner.AddCommandHandler("SetSprite", UseSprite);
        dialogueRunner.AddCommandHandler("SpriteDisable", SpriteDisable);
    }

    public void UseSprite(string[] parameters)
    {
        Debug.Log("Setting sprite");
        Sprite s = null;
        foreach (var info in sprites)
        {
            if (info.name == parameters[0])
            {
                s = info.sprite;
                Debug.Log("Set sprite " + info.name);
                break;
            }
        }
        if (s == null)
        {
            Debug.LogErrorFormat("Can't find sprite named {0}!", parameters[0]);
            return;
        }

        GetComponent<Image>().sprite = s;
    }

    public void SpriteDisable(string[] parameters)
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

}
