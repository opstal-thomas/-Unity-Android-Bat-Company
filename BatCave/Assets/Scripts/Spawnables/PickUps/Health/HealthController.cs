using UnityEngine;

public class HealthController : Powerup {
    private ParticleSystem particle;
    private SpriteRenderer spriteRenderer;

    private bool isHitByPlayer;
    private bool isEmittingBlood;

    void Start() {
        base.BaseStart();
        particle = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (isHitByPlayer) {
            if (!isEmittingBlood) {
                StartBloodParticles();
                isEmittingBlood = true;
                isHitByPlayer = false;
            } else {
                if (!particle.isPlaying) {
                    OnParticleAnimFinished();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (!isHitByPlayer) {
            if (col.gameObject.tag == "Player") {
                PlayRandomSound();
                EventManager.TriggerEvent(EventTypes.HEALTH_PICKED_UP);
                isHitByPlayer = true;
                spriteRenderer.enabled = false;
            }
        }
        if (col.gameObject.tag == "CleanUp") {
            spriteRenderer.enabled = true;
        }
    }

    void StartBloodParticles() {
        particle.Play();
    }

    void OnParticleAnimFinished() {
        isEmittingBlood = false;
    }
}
