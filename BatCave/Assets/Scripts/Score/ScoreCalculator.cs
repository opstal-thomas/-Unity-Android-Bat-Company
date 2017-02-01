using UnityEngine;
using System.Collections.Generic;

public class ScoreCalculator : MonoBehaviour {
    private float timePlayed = 0;
    public float scoreMultiplier;
    public float playerScore;

    private bool isPaused;
    private bool gameStarted;


    void Start () {
        EventManager.StartListening(EventTypes.GAME_OVER, OnGameOver);
        EventManager.StartListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StartListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StartListening(EventTypes.PLAYER_DIED, OnGamePaused);
        EventManager.StartListening(EventTypes.PLAYER_IN_POSITION, OnIntroCompleet);
        EventManager.StartListening(PowerupEvents.PLAYER_SPEED_PICKUP, SetSpeedMultiplier);
        EventManager.StartListening(PowerupEvents.PLAYER_SPEED_ENDED, SetSpeedMultiplier);
    }

    void OnGamePaused(object arg0) {
        isPaused = true;
    }

    void OnGameResume(object arg0) {
        isPaused = false;
    }

    void OnIntroCompleet(object arg0) {
        gameStarted = true;
    }

    // Update is called once per frame
    void Update () {
        if (!isPaused) {
            if (scoreMultiplier < 0) {
                scoreMultiplier = 1;
            }

            if (playerScore < 0) {
                playerScore = 0;
            }
        }
	}

    void OnDestroy() {
        EventManager.StopListening(EventTypes.GAME_OVER, OnGameOver);
        EventManager.StopListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StopListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StopListening(EventTypes.PLAYER_DIED, OnGamePaused);
        EventManager.StopListening(EventTypes.PLAYER_IN_POSITION, OnIntroCompleet);
        EventManager.StopListening(PowerupEvents.PLAYER_SPEED_PICKUP, SetSpeedMultiplier);
        EventManager.StopListening(PowerupEvents.PLAYER_SPEED_ENDED, SetSpeedMultiplier);
    }

    void FixedUpdate() {
        if (!isPaused && gameStarted) 
            playerScore += 1 * scoreMultiplier;
    }

    void OnGameOver(object arg0) {
        SaveLoadController.GetInstance().GetEndlessSession().SetScore(playerScore);
    }

    void SetSpeedMultiplier(object arg0) {
        if (scoreMultiplier == 3) {
            scoreMultiplier = 1;
        } else {
            scoreMultiplier = 3;
        }
    }
}
