using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimpleMoveScript : MonoBehaviour {
    public float startSpeed;
    public Vector2 Speed = new Vector2(0.0f, -0.05f);
    private Rigidbody2D rb;
    private bool isPaused;
    private bool isIntro = true;
    //public float speed;
    //public Vector3 direction;
    private float speedIncreaseTime = 360;
    public float maxSpeedIncrease;
    public int amountOfSpeedIncrements = 4;
    private bool increaseSpeed = true;

    private bool SpeedBoostActive;
    public Vector2 BoostSpeed = new Vector2(0, -0.2f);

    void Start() {
        Speed = new Vector2(0, startSpeed);

        EventManager.StartListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StartListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StartListening(EventTypes.PLAYER_DIED, OnGamePaused);
        EventManager.StartListening(EventTypes.PLAYER_IN_POSITION, OnIntroCompleet);
        EventManager.StartListening(PowerupEvents.PLAYER_SPEED_PICKUP, ActivateSpeedBoost);
        EventManager.StartListening(PowerupEvents.PLAYER_SPEED_ENDED, DeactivateSpeedBoost);

        // GPMP
        EventManager.StartListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnGameStartReady);
        EventManager.StartListening(GPMPEvents.Types.GPMP_PLAYER_DIED.ToString(), OnGamePaused);
        EventManager.StartListening(GPMPEvents.Types.GPMP_OPPONENT_DIED.ToString(), OnGamePaused);

        StartCoroutine(StartTimer());
    }

    void OnGamePaused(object arg0) {
        isPaused = true;
    }

    void OnGameResume(object arg0) {
        isPaused = false;
    }

    void OnIntroCompleet(object arg0) {
        isIntro = false;
    }

    void FixedUpdate() {
        if (!isPaused && !isIntro) {
            if (SpeedBoostActive)
                transform.Translate(BoostSpeed.x, BoostSpeed.y, 0);
            else 
                transform.Translate(Speed.x, Speed.y, 0);
        }
    }

    void OnDestroy() {
        //EventManager.StopListening(EventTypes.PLAYER_SPEED_CHANGED, OnSpeedChanged);
        EventManager.StopListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StopListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StopListening(EventTypes.PLAYER_DIED, OnGamePaused);
        EventManager.StopListening(EventTypes.PLAYER_IN_POSITION, OnIntroCompleet);
        EventManager.StopListening(PowerupEvents.PLAYER_SPEED_PICKUP, ActivateSpeedBoost);
        EventManager.StopListening(PowerupEvents.PLAYER_SPEED_ENDED, DeactivateSpeedBoost);

        // GPMP
        EventManager.StopListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnGameStartReady);
        EventManager.StopListening(GPMPEvents.Types.GPMP_PLAYER_DIED.ToString(), OnGamePaused);
        EventManager.StopListening(GPMPEvents.Types.GPMP_OPPONENT_DIED.ToString(), OnGamePaused);
    }

    // GPMP
    private void OnGameStartReady(object arg0) {
        isPaused = false;
        isIntro = false;
    }

    IEnumerator StartTimer() {
        //speedIncreaseTime -= Time.deltaTime;
        while (increaseSpeed) {
            yield return new WaitForSeconds(speedIncreaseTime / amountOfSpeedIncrements);

            Speed = new Vector2(0, Speed.y - (maxSpeedIncrease/amountOfSpeedIncrements));

            if (Speed.y >= (startSpeed + maxSpeedIncrease))
            {
                increaseSpeed = false;
            }
        }
    }

    public void ActivateSpeedBoost(object arg0) {
        SpeedBoostActive = true;
    }

    public void DeactivateSpeedBoost(object arg0) {
        SpeedBoostActive = false;
    }
}
