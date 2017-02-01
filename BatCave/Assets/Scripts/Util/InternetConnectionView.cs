using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InternetConnectionView : MonoBehaviour {

    public float testConnectionInterval;
    public Sprite noConnectionSprite;
    public Sprite limitedConnectionSprite;
    private Image buttonGraphic;
    private Button button;

	void Start () {
        buttonGraphic = GetComponent<Image>();
        button = GetComponent<Button>();
        buttonGraphic.enabled = false;
        EventManager.StartListening(InternetConnectionStatus.CONNECTION_STATUS_UPDATE, OnConnectionStatusUpdate);
        InvokeRepeating("DispatchTestConnection", 0f, 4f);
	}
	
	void OnDestroy () {
        EventManager.StopListening(InternetConnectionStatus.CONNECTION_STATUS_UPDATE, OnConnectionStatusUpdate);
        CancelInvoke();
    }

    void OnConnectionStatusUpdate(object s) {
        InternetConnectionStatus.Status status = (InternetConnectionStatus.Status)s;
        button.onClick.RemoveAllListeners();
        buttonGraphic.enabled = true;
        switch (status) {
            case InternetConnectionStatus.Status.CONNECTED:
                buttonGraphic.enabled = false;
                break;
            case InternetConnectionStatus.Status.LIMITED:
                buttonGraphic.sprite = limitedConnectionSprite;
                button.onClick.AddListener(() => { OnTargetClicked(); });
                break;
            case InternetConnectionStatus.Status.NO_CONNECTION:
                buttonGraphic.sprite = noConnectionSprite;
                button.onClick.AddListener(() => { OnTargetClicked(); });
                break;
            case InternetConnectionStatus.Status.UNKNOWN:
                buttonGraphic.sprite = limitedConnectionSprite;
                button.onClick.AddListener(() => { OnTargetClicked(); });
                break;
        }
    }

    void OnTargetClicked() {
        EventManager.TriggerEvent(InternetConnectionStatus.SHOW_CONNECTION_STATE);
    }

    void DispatchTestConnection() {
        EventManager.TriggerEvent(InternetConnectionStatus.TEST_CONNECTION_STATUS);
    }
}
