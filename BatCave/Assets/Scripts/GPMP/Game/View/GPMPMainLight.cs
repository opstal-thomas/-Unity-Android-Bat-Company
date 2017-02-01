using UnityEngine;
using System.Collections;
using System;

public class GPMPMainLight : MonoBehaviour {

    Light light;

    private bool isFadingOut;
    private bool isFadingIn;
    public float fadeSpeed;

	void Start () {
        light = GetComponent<Light>();
        EventManager.StartListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnGameStarted);
    }

    void OnDestroy() {
        EventManager.StopListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnGameStarted);
    }

    void Update() {
        if (isFadingOut) {
            light.intensity = Mathf.Lerp(light.intensity, 0f, fadeSpeed * Time.deltaTime);

            if (light.intensity <= 0.50f) {
                light.intensity = 0f;
                isFadingOut = false;
            }
        }

        if (isFadingIn) {
            light.intensity = Mathf.Lerp(light.intensity, 8f, fadeSpeed * Time.deltaTime);

            if (light.intensity >= 3.5f) {
                light.intensity = 4f;
                isFadingIn = false;
            }
        }
    }

    private void OnGameStarted(object arg0) {
        isFadingOut = true;
    }
}
