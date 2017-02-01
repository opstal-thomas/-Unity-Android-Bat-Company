using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SaveLoadView : MonoBehaviour {

    public GameObject panel;
    public Text infoText;

	// Use this for initialization
	void Start () {
        EventManager.StartListening(EventTypes.DONE_LOADING_SAVE_DATA, OnDoneLoadingSaveGame);
        EventManager.StartListening(EventTypes.LOADING_SAVE_DATA, OnStartLoadingSaveData);
        EventManager.StartListening(EventTypes.START_SAVING_GAME, OnStartSavingGame);
        EventManager.StartListening(EventTypes.DONE_SAVING_GAME, OnSavingGameDone);
    }

    void Update() {
        if (GooglePlayHelper.GetInstance().isLoading)
            OnStartLoadingSaveData(null);
        if (GooglePlayHelper.GetInstance().isSaving)
            OnStartSavingGame(null);
    }

    void OnDestroy() {
        EventManager.StopListening(EventTypes.DONE_LOADING_SAVE_DATA, OnDoneLoadingSaveGame);
        EventManager.StopListening(EventTypes.LOADING_SAVE_DATA, OnStartLoadingSaveData);
        EventManager.StopListening(EventTypes.START_SAVING_GAME, OnStartSavingGame);
        EventManager.StopListening(EventTypes.DONE_SAVING_GAME, OnSavingGameDone);
    }

    private void OnSavingGameDone(object arg0) {
        panel.SetActive(false);
    }

    private void OnStartSavingGame(object arg0) {
        panel.SetActive(true);
        infoText.text = "Saving game...";
    }

    private void OnStartLoadingSaveData(object arg0) {
        panel.SetActive(true);
        infoText.text = "Loading game...";
    }

    private void OnDoneLoadingSaveGame(object arg0) {
        panel.SetActive(false);
    }
}
