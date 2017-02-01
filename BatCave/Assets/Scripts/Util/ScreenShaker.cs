using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenShaker : MonoBehaviour {
    
    private float shakeY = 0;
    private float shakeYSpeed = 0.8f;
    private float resetShakeY = 0.8f;
    private float shakeCounter = 2f;

    void Start() {
        EventManager.StartListening(EventTypes.PLAYER_TAKES_DAMAGE, OnPlayerTakesDamage);

        // GPMP
        EventManager.StartListening(GPMPEvents.Types.GPMP_PLAYER_DIED.ToString(), OnPlayerTakesDamage);
    }

    void OnDestroy() {
        EventManager.StopListening(EventTypes.PLAYER_TAKES_DAMAGE, OnPlayerTakesDamage);

        // GPMP
        EventManager.StopListening(GPMPEvents.Types.GPMP_PLAYER_DIED.ToString(), OnPlayerTakesDamage);
    }

    void OnPlayerTakesDamage(object arg0) {
        Shake();
    }

    void Shake() {
        shakeY = resetShakeY;
        shakeCounter = 3f;
    }

    void Update() {
        if (shakeCounter > 0) {
            shakeCounter -= Time.deltaTime;
            Vector2 newPosition = new Vector2(0, shakeY);
            if (shakeY < 0)
            {
                shakeY *= shakeYSpeed;
            }
            shakeY = -shakeY;
            transform.Translate(newPosition, Space.Self);
        }
    }
}
