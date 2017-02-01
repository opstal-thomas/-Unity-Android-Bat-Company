using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;

public class GPMPVersusScreenView : MonoBehaviour {

    public Text playerText;
    public Text opponentText;
    public float screenDuration;

	// Use this for initialization
	void Start () {
        string myID = PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
        List<Participant> players = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
        foreach (Participant p in players) {
            if (p.ParticipantId == myID)
                playerText.text = p.DisplayName;
            else
                opponentText.text = p.DisplayName;
        }
        StartCoroutine("TriggerGameOverScreen");
    }

    IEnumerator TriggerGameOverScreen() {
        yield return new WaitForSeconds(screenDuration);
        LoadingController.LoadScene(LoadingController.Scenes.GPMP_GAME);
    }
}
