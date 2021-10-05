using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTileController : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    private const float total = 3.0f;

    private float timer = -1.0f;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartUp(Sprite sprite, Color color) {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;

        timer = 0.0f;
    }

    private void Update() {
        if (timer < 0.0f) {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= total) {
            timer = -1;
            DestroyImmediate(gameObject, true);
        }
        else {
            float alpha = 1.0f - timer / total;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        }
    }
}
