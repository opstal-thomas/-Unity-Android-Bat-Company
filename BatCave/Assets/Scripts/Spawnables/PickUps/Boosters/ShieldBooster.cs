using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldBooster : Powerup
{
    public float shieldDuration;
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
            EventManager.TriggerEvent(PowerupEvents.PLAYER_SHIELD_PICKUP, shieldDuration);
        }
        if (col.gameObject.tag == "CleanUp") {
            spriteRenderer.enabled = true;
        }
    }
}
