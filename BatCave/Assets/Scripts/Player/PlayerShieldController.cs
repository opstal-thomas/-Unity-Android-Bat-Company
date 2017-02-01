using UnityEngine;
using System.Collections.Generic;

public class PlayerShieldController : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    public float blinkInterval;
    public bool powerActive;

    private float blinkIntervalCounter;
    private bool isBlinking;
    private float shieldDuration;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        EventManager.StartListening(PowerupEvents.PLAYER_SHIELD_PICKUP, OnShieldActivated);
        EventManager.StartListening(PowerupEvents.PLAYER_SHIELD_ENDED, OnsShieldEnded);
    }

    void OnDestroy() {
        EventManager.StopListening(PowerupEvents.PLAYER_SHIELD_PICKUP, OnShieldActivated);
        EventManager.StopListening(PowerupEvents.PLAYER_SHIELD_ENDED, OnsShieldEnded);
    }

    void OnShieldActivated(object duration) {
        spriteRenderer.enabled = true;
        powerActive = true;
        shieldDuration = (float)duration;
        isBlinking = false;
    }

    void OnsShieldEnded(object arg0) {
        isBlinking = false;
        blinkIntervalCounter = 0;
        spriteRenderer.enabled = false;
        powerActive = false;
    }


    void Update() {
        if (shieldDuration < 3f && !isBlinking && powerActive) {
            isBlinking = true;
        } else {
            shieldDuration -= Time.deltaTime;
        }

        if (isBlinking) {
            blinkIntervalCounter += Time.deltaTime;
            if (blinkIntervalCounter >= blinkInterval) {
                ToggleSprite();
                blinkIntervalCounter = 0;
            }
        }
    }

    void ToggleSprite() {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }
}
