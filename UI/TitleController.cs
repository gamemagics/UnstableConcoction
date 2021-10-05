using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleController : MonoBehaviour
{
    public GameObject arrow, start, exit, banner, potion;
    public Ease animEase;

    [SerializeField] private AudioSource effectAudio;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        potion.transform.DOMoveY(0.6f, 1f).From(8).SetEase(animEase).OnComplete(()=>{
            effectAudio.Play();
        });
        yield return new WaitForSeconds(0.5f);
        banner.transform.DOMoveY(0, 1f).From(8).SetEase(animEase).OnComplete(()=>{
            effectAudio.Play();
        });
    }

    public void StartGame()
    {
        Booter.instance.Change(3);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void MoveUp()
    {
        arrow.transform.position = new Vector3(arrow.transform.position.x,
                                               start.transform.position.y,
                                               arrow.transform.position.z);
    }
    public void MoveDown()
    {
        arrow.transform.position = new Vector3(arrow.transform.position.x,
                                               exit.transform.position.y,
                                               arrow.transform.position.z);
    }
}
