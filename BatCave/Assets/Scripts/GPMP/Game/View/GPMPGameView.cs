using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Collections;

public class GPMPGameView : MonoBehaviour {

    public GameObject pauseGamePanel;

    void Start() {
        EventManager.StartListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnMatchStarted);
    }

    void OnDestroy() {
        EventManager.StopListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnMatchStarted);
    }

    private void OnMatchStarted(object arg0) {
        pauseGamePanel.SetActive(false);

    }
    
    public void LeaveGame() {
        EventManager.TriggerEvent(GPMPEvents.Types.GPMP_LEAVE_GAME.ToString());
    }
}
