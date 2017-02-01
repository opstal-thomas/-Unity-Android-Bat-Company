using UnityEngine;
using System.Collections.Generic;

public class LightPowerController : MonoBehaviour {

    private ParticleSystem particleSystem;

    void Start() {
        particleSystem = GetComponent<ParticleSystem>();
        EventManager.StartListening(PowerupEvents.PLAYER_LIGHT_PICKUP, OnLightPowerPickedUp);
    }

    void OnDestroy() {
        EventManager.StopListening(PowerupEvents.PLAYER_LIGHT_PICKUP, OnLightPowerPickedUp);
    }

    void OnLightPowerPickedUp(object arg0) {
        particleSystem.Play();
    }
}
