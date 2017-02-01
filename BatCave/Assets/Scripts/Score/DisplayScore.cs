using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DisplayScore : MonoBehaviour {
    public ScoreCalculator score;

    private Text text;
    public Text coinText;
    public Image coinImage;
    private bool isPaused;

    private Animator coinAnimator;

    private EndlessSessionSave ess;

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
        text.text = "0";
        ess = SaveLoadController.GetInstance().GetEndlessSession();
        coinAnimator = coinImage.GetComponent<Animator>();
        EventManager.StartListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StartListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StartListening(PowerupEvents.PLAYER_COIN_PICKUP, OnCoinPickedUp);
    }

    private void OnCoinPickedUp(object arg0) {
        coinAnimator.SetTrigger("Play");
    }

    // Update is called once per frame
    void Update () {
        if (!isPaused) {
            text.text = Mathf.FloorToInt(score.playerScore).ToString();
            coinText.text = ess.GetTotalCoinsCollected().ToString();
        }
	}

    void OnDestroy() {
        EventManager.StopListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StopListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StopListening(PowerupEvents.PLAYER_COIN_PICKUP, OnCoinPickedUp);
    }

    void OnGamePaused(object arg0) {
        isPaused = true;
    }

    void OnGameResume(object arg0) {
        isPaused = false;
    }
}
