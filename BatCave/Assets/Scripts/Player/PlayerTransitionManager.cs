using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerTransitionManager : MonoBehaviour {
    public PlayerControls playerControls;
    public GameObject echo;

    private void Start() {
        EventManager.StartListening(EventTypes.HIDE_AND_DISABLE, HideAndDisable);
        EventManager.StartListening(EventTypes.SHOW_AND_ENABLE, ShowAndEnable);

        playerControls.echoEnabled = false;
        echo.SetActive(false);
    }

    private void OnDestroy() {
        EventManager.StopListening(EventTypes.HIDE_AND_DISABLE, HideAndDisable);
        EventManager.StopListening(EventTypes.SHOW_AND_ENABLE, ShowAndEnable);
    }

    private void HideAndDisable(object value) {
        playerControls.echoEnabled = false;
        echo.SetActive(false);
    }

    private void ShowAndEnable(object value) {
        playerControls.echoEnabled = true;
        echo.SetActive(true);
    }
}
