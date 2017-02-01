using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class InternetConnectionPopup : MonoBehaviour {

    public GameObject panel;
    public Text infoText;
    public InternetConnectionStatus ics;

	// Use this for initialization
	void Start () {
        panel.SetActive(false);
        EventManager.StartListening(InternetConnectionStatus.SHOW_CONNECTION_STATE, OnShowConnectionStatus);
	}
	
	// Update is called once per frame
	void OnDestroy () {
        EventManager.StopListening(InternetConnectionStatus.SHOW_CONNECTION_STATE, OnShowConnectionStatus);
    }

    private void OnShowConnectionStatus(object arg0) {
        panel.SetActive(true);
        infoText.text = ics.GetCurrentConnectionStatusInfo();
    }

    public void Hide() {
        panel.SetActive(false);
    }
}
