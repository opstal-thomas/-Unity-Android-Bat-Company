using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using GooglePlayGames;

public class GPMPLobbyView : MonoBehaviour {

    public GPMPMatchModel matchModel;
    public GameObject errorMessagePanel;
    public Text errorMessageText;
    public Button errorPanelButton;
    public Text errorPanelButtonText;
    public Button toMainMenuButton;
    public GameObject inviteIndicator;
    public Text invitationAmountText;

    private int invitationAmount = 0;

    private bool internetAvailable = true;
    //GPMP_INVITATION_RECIEVED
    void Start() {
        EventManager.StartListening(GPMPEvents.Types.GPMP_SHOW_ERROR_MESSAGE.ToString(), OnErrorMessageRecieved);
        EventManager.StartListening(InternetConnectionStatus.CONNECTION_STATUS_UPDATE, OnConnectionStatusUpdated);

        try {
            if (PlayGamesPlatform.Instance.RealTime.IsRoomConnected())
                PlayGamesPlatform.Instance.RealTime.LeaveRoom();
            if (!GooglePlayHelper.GetInstance().IsPlayerAuthenticated())
                SetMessageForLogin();
        } catch(Exception e) {
            SetMessageForLogin();
        }

        InvokeRepeating("OnDisplayInvites", 0f, 1f);
    }

    void OnDestroy() {
        EventManager.StopListening(GPMPEvents.Types.GPMP_SHOW_ERROR_MESSAGE.ToString(), OnErrorMessageRecieved);
        EventManager.StopListening(InternetConnectionStatus.CONNECTION_STATUS_UPDATE, OnConnectionStatusUpdated);
        CancelInvoke("OnDisplayInvites");
    }

    private void OnDisplayInvites() {
        PlayGamesPlatform.Instance.RealTime.GetAllInvitations((invites) => {
            invitationAmount = invites.Length;
        });

        if (invitationAmount > 0) {
            inviteIndicator.SetActive(true);
            invitationAmountText.text = invitationAmount.ToString();
        } else {
            inviteIndicator.SetActive(false);
        }
    }

    private void OnConnectionStatusUpdated(object s) {
        InternetConnectionStatus.Status status = (InternetConnectionStatus.Status)s;
        switch (status) {
            case InternetConnectionStatus.Status.LIMITED:
            case InternetConnectionStatus.Status.CONNECTED:
                internetAvailable = true;
                break;
            case InternetConnectionStatus.Status.NO_CONNECTION:
            case InternetConnectionStatus.Status.UNKNOWN:
                internetAvailable = false;
                break;
        }
    }

    private void OnErrorMessageRecieved(object m) {
        string message = (string)m;
        errorMessagePanel.gameObject.SetActive(true);
        toMainMenuButton.gameObject.SetActive(false);
        errorMessageText.text = message;
        errorPanelButton.onClick.RemoveAllListeners();
        UnityEngine.Events.UnityAction action = () => { ConfirmMessage(); };
        errorPanelButton.onClick.AddListener(action);
        errorPanelButtonText.text = "OK";
    }

    void SetMessageForLogin() {
        errorMessagePanel.SetActive(true);
        toMainMenuButton.gameObject.SetActive(true);
        errorMessageText.text = "You must be signed in to play online";
        errorPanelButton.onClick.RemoveAllListeners();
        UnityEngine.Events.UnityAction action = () => { Login(); };
        errorPanelButton.onClick.AddListener(action);
        errorPanelButtonText.text = "Login";
    }

    public void StartQuickMatch() {
        if (internetAvailable)
            EventManager.TriggerEvent(GPMPEvents.Types.GPMP_SEARCH_QUICK_MATCH.ToString(), matchModel);
        else
            EventManager.TriggerEvent(InternetConnectionStatus.SHOW_CONNECTION_STATE);
    }

    public void StartWithInvites() {
        if (internetAvailable)
            EventManager.TriggerEvent(GPMPEvents.Types.GPMP_START_WITH_INVITE.ToString(), matchModel);
        else
            EventManager.TriggerEvent(InternetConnectionStatus.SHOW_CONNECTION_STATE);
    }

    public void ShowAllInvites() {
        if (internetAvailable)
            EventManager.TriggerEvent(GPMPEvents.Types.GPMP_VIEW_INVITES.ToString());
        else
            EventManager.TriggerEvent(InternetConnectionStatus.SHOW_CONNECTION_STATE);
    }

    public void ToMainMenu() {
        LoadingController.LoadScene(LoadingController.Scenes.MAIN_MENU);
    }

    public void Login() {
        OnErrorMessageRecieved("Click OK if you are logged in");
        GooglePlayHelper.GetInstance().Login();

    }

    public void ConfirmMessage() {
        errorMessagePanel.SetActive(false);
        if (!GooglePlayHelper.GetInstance().IsPlayerAuthenticated())
            SetMessageForLogin();
    }
}
