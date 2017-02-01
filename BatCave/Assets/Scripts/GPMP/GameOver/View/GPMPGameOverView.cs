using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPMPGameOverView : MonoBehaviour {

    public Text statusText;
    public float screenDuration;

    // Use this for initialization
    void Start() {
        MultiplayerSessionSave mss = SaveLoadController.GetInstance().GetMultiplayerSession();

        // Player won
        if (mss.GetWinningPlayer() == mss.GetPlayer()) {
            statusText.text = "You won against " + mss.GetOpponent().GetDisplayName() + "!";
        } else {
            // player lost
            statusText.text = "You lost against " + mss.GetOpponent().GetDisplayName();
        }

        StartCoroutine("TriggerGameOverScreen");
    }

    IEnumerator TriggerGameOverScreen() {
        yield return new WaitForSeconds(screenDuration);
        EventManager.TriggerEvent(GPMPEvents.Types.GPMP_LEAVE_GAME.ToString());
    }
}
