using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnableDisableAudio : MonoBehaviour {
    public Button audioButton;
    public Sprite audioEnabled;
    public Sprite audioDisabled;
    private int audioState;

	// Use this for initialization
	void Start () {
        audioState = PlayerPrefs.GetInt("ToggleAudio");
        EventManager.StartListening(EventTypes.ENABLE_AUDIO, EnableAudio);
        EventManager.StartListening(EventTypes.DISABLE_AUDIO, DisableAudio);

        if (audioState == 1)
        {
            AudioListener.volume = 0;
            if (audioButton != null)
            {
                audioButton.GetComponent<Image>().sprite = audioDisabled;
            }
        }
        else
        {
            AudioListener.volume = 1;
            if (audioButton != null)
            {
                audioButton.GetComponent<Image>().sprite = audioEnabled;
            }
        }
	}

    private void EnableAudio(object arg0) {
        if (audioButton != null) {
            audioButton.GetComponent<Image>().sprite = audioEnabled;
        }

        AudioListener.volume = 1;
        audioState = 0;
        PlayerPrefs.SetInt("ToggleAudio", audioState);
    }

    private void DisableAudio(object arg0) {
        if (audioButton != null)
        {
            audioButton.GetComponent<Image>().sprite = audioDisabled;
        }

        AudioListener.volume = 0;
        audioState = 1;
        PlayerPrefs.SetInt("ToggleAudio", audioState);
    }

    private void OnDestroy() {
        EventManager.StopListening(EventTypes.ENABLE_AUDIO, EnableAudio);
        EventManager.StopListening(EventTypes.DISABLE_AUDIO, DisableAudio);
    }
}
