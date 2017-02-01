using UnityEngine;
using System.Collections;
using System;

public class DayNightCycle : MonoBehaviour {
    public Light playerLight;
    public Light DayTimeLight;
    public float fadeOutSpeed;
    public float speedFade;
    public float speedFadeOut;
    public float speedAngleFadeOut;
    private bool isFadingOut = false;
    private bool isFadingIn = false;
    private bool speedBoosterActive = false;
    private bool speedBoosterFadeOut = false;
    private bool lightPowerActive = false;

    void Start() {
        EventManager.StartListening(EventTypes.FADE_LIGHT_IN, OnTransitionStart);
        EventManager.StartListening(EventTypes.FADE_LIGHT_OUT, OnTransitionEnd);
        EventManager.StartListening(PowerupEvents.PLAYER_LIGHT_PICKUP, OnPlayerPickedUpLight);
        EventManager.StartListening(PowerupEvents.PLAYER_LIGHT_ENDED, OnPlayerLightPowerEnded);
    }

    private void OnPlayerLightPowerEnded(object arg0) {
        lightPowerActive = false;
    }

    private void OnPlayerPickedUpLight(object arg0) {
        lightPowerActive = true;
    }

    void OnTransitionEnd(object arg0) {
        setNightTime();
    }

    void OnTransitionStart(object arg0) {
        setDayTime();
    }

    void OnDestroy() {
        EventManager.StopListening(EventTypes.FADE_LIGHT_IN, OnTransitionStart);
        EventManager.StopListening(EventTypes.FADE_LIGHT_OUT, OnTransitionEnd);
    }

    public void setDayTime() {
        isFadingIn = true;
    }

    public void setNightTime() {
        isFadingOut = true;
    }

    void Update() {
        if (isFadingOut && !speedBoosterActive && !lightPowerActive) {
            DayTimeLight.intensity = Mathf.Lerp(DayTimeLight.intensity, 0f, fadeOutSpeed * Time.deltaTime);
            playerLight.intensity = Mathf.Lerp(playerLight.intensity, 8f, fadeOutSpeed * Time.deltaTime);

            if (DayTimeLight.intensity <= 0.50f && playerLight.intensity >= 5f) {
                playerLight.intensity = 8f;
                DayTimeLight.intensity = 0f;
                isFadingOut = false;
            }
        }

        if (isFadingIn) {
            DayTimeLight.intensity = Mathf.Lerp(DayTimeLight.intensity, 8f, fadeOutSpeed * Time.deltaTime);
            playerLight.intensity = Mathf.Lerp(playerLight.intensity, 0f, fadeOutSpeed * Time.deltaTime);

            if (DayTimeLight.intensity >= 3.5f && playerLight.intensity >= 0.50f) {
                playerLight.intensity = 0f;
                DayTimeLight.intensity = 4f;
                isFadingIn = false;
            }
        }

        if (speedBoosterActive) {
            playerLight.spotAngle = Mathf.Lerp(playerLight.spotAngle, 179, speedFade * Time.deltaTime);
            playerLight.range = Mathf.Lerp(playerLight.range, 25, speedFade * Time.deltaTime);

            if (playerLight.spotAngle == 179 && playerLight.range == 25) {
                speedBoosterActive = false;
            }
        }

        if (speedBoosterFadeOut) {
            playerLight.spotAngle -= speedAngleFadeOut;
            playerLight.range -= speedFadeOut;

            if (playerLight.spotAngle <= 87){
                playerLight.spotAngle = 87;
            }

            if (playerLight.range <= 10) {
                playerLight.range = 10;
            }

            if (playerLight.spotAngle <= 87 && playerLight.range <= 10) {
                speedBoosterFadeOut = false;
            }
        }
    }

    public void IncreasePlayerLightRange() {
        speedBoosterActive = true;
    }

    public void DecreasePlayerLightRange() {
        speedBoosterActive = false;
        speedBoosterFadeOut = true;
    }
}
