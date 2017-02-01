using System.Collections;
using UnityEngine;

public class GPMPLobbyController : MonoBehaviour {

    void Start() {
        DebugMP.Log("Player left on purpose: " + GPMPController.playerLeftOnPurpose);
        if (!GPMPController.playerLeftOnPurpose) {
            Invoke("TriggerErrorMessageWithDelay", 0.1f);
        }
    }

    void TriggerErrorMessageWithDelay() {
        // call for error message
        EventManager.TriggerEvent(GPMPEvents.Types.GPMP_SHOW_ERROR_MESSAGE.ToString(), GPMPController.errorMessage);

        // reset error trigger
        GPMPController.playerLeftOnPurpose = true;
    }
}
