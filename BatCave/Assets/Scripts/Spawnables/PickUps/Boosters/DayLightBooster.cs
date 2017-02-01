using UnityEngine;

public class DayLightBooster : Powerup
{
    public float DayTimeUpTime;
    private SpriteRenderer spriteRenderer;

    void Start() {
        base.BaseStart();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            spriteRenderer.enabled = false;
            PlayRandomSound();
            EventManager.TriggerEvent(PowerupEvents.PLAYER_LIGHT_PICKUP, DayTimeUpTime);
        }
        if (col.gameObject.tag == "CleanUp") {
            spriteRenderer.enabled = true;
        }
    }
}
