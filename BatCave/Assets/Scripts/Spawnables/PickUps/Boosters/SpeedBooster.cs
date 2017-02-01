using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class SpeedBooster : Powerup
{

    public float speedDuration;
    private SpriteRenderer spriteRenderer;

    void Start() {
        base.BaseStart();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player")
        {
            spriteRenderer.enabled = false;
            PlayRandomSound();
            EventManager.TriggerEvent(PowerupEvents.PLAYER_SPEED_PICKUP, speedDuration);
        }
        if (col.gameObject.tag == "CleanUp") {
            spriteRenderer.enabled = true;
        }
    }
}
