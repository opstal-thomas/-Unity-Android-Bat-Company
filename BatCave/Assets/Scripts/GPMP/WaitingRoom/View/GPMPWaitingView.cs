using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GPMPWaitingView : MonoBehaviour {

    public Text statusTextField;

    void Start() {
        EventManager.StartListening(GPMPEvents.Types.GPMP_REPORT_ROOM_SETUP_PROGRESS.ToString(), OnProgressStatusUpdate);
        EventManager.StartListening(GPMPEvents.Types.GPMP_OPPONENT_FOUND.ToString(), OnOpponentFound);
    }

    void OnDestroy() {
        EventManager.StopListening(GPMPEvents.Types.GPMP_REPORT_ROOM_SETUP_PROGRESS.ToString(), OnProgressStatusUpdate);
        EventManager.StopListening(GPMPEvents.Types.GPMP_OPPONENT_FOUND.ToString(), OnOpponentFound);
    }

    private void OnOpponentFound(object arg0) {
        statusTextField.text = "Opponent connected. Starting match.";
    }

    public void CancelMatching() {
        EventManager.TriggerEvent(GPMPEvents.Types.GPMP_CANCEL_MATCH_MAKING.ToString());
    }

    void OnProgressStatusUpdate(object percentage) {
        //statusTextField.text = ((float)percentage).ToString();
        statusTextField.text = "Game ready. Now waiting for opponent.";
    }
}
