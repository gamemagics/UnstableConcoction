using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PressButtonEffect : MonoBehaviour
{
    //private Button button;
    private RectTransform text = null;
    public float delta = 5f;
    float posY;
    // Start is called before the first frame update
    void Start()
    {
        //button = this.GetComponent<Button>();
        text = this.GetComponent<RectTransform>();
        posY = text.localPosition.y;
    }

    public void PressButton()
    {
        text.DOLocalMoveY(posY - delta, 0);
    }
    public void ResetText()
    {
        text.DOLocalMoveY(posY, 0);
    }
}
