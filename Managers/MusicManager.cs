using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField] private AudioSource effect;
    
    [SerializeField] private AudioSource bgm;

    [SerializeField] private AudioClip[] clips;

    private void Awake() {
        MusicEventManager.musicEvent.AddListener(Play);
    }

    private void Play(string name) {
        if (clips == null) {
            Debug.LogError("No Clips!");
            return;
        }

        foreach (var clip in clips) {
            if (clip.name == name) {
                bgm.volume = 0.5f;
                effect.clip = clip;
                effect.Play();
                break;
            }
        }
        
    }

    private void Update() {
        if (!effect.isPlaying) {
            bgm.volume = 1.0f;
        }
    }

    private void OnDestroy() {
        MusicEventManager.musicEvent.RemoveAllListeners();
    }

}
