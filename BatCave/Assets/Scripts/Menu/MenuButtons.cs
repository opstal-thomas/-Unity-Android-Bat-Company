using UnityEngine;

public class MenuButtons : MonoBehaviour {

    private bool internetAvailable = true;

    void Start() {
        EventManager.StartListening(InternetConnectionStatus.CONNECTION_STATUS_UPDATE, OnConnectionStatusUpdated);

        Debug.Log("PlayerRefusedGooglePLay: " + PlayerPrefs.GetInt("PlayerRefusedGooglePLay"));
        if (!PlayerPrefs.HasKey("PlayerRefusedGooglePLay"))
            PlayerPrefs.SetInt("PlayerRefusedGooglePLay", 0);

        if (!Social.localUser.authenticated && !Application.isEditor && PlayerPrefs.GetInt("PlayerRefusedGooglePLay") == 0) {
            GooglePlayHelper.GetInstance().Login();
        }
    }

    void OnDestroy() {
        EventManager.StopListening(InternetConnectionStatus.CONNECTION_STATUS_UPDATE, OnConnectionStatusUpdated);
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

    public void ShowAchievementsUI() {
        if (!Social.localUser.authenticated && !Application.isEditor) {
            GooglePlayHelper.GetInstance().Login();
        } else {
            GooglePlayHelper.GetInstance().ShowAchievementsUI();
        }
        
    }

    public void ShowLeaderboardUI() {
        if (!Social.localUser.authenticated && !Application.isEditor) {
            GooglePlayHelper.GetInstance().Login();
        } else {
            GooglePlayHelper.GetInstance().ShowLeaderboardUI();
        }
    }

    public void StartEndless()
    {
        MenuTheme.Instance.StopAudio();
        LoadingController.LoadScene(LoadingController.Scenes.GAME);
    }

    public void StartArcade()
    {

    }

    public void StartInfoMenu() {
        LoadingController.LoadScene(LoadingController.Scenes.INFO_MENU);
    }

    public void OpenShop() { 
        LoadingController.LoadScene(LoadingController.Scenes.STORE);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        LoadingController.LoadScene(LoadingController.Scenes.GAME);
    }

    public void MainMenu()
    {
        LoadingController.LoadScene(LoadingController.Scenes.MAIN_MENU);
    }

    public void Multiplayer() {
        MenuTheme.Instance.StopAudio();

        if (internetAvailable)
            LoadingController.LoadScene(LoadingController.Scenes.GPMP_LOBBY);
        else
            EventManager.TriggerEvent(InternetConnectionStatus.SHOW_CONNECTION_STATE);
    }

    public void Leaderboards()
    {

    }

    public void ToggleAudio() {
        if (PlayerPrefs.GetInt("ToggleAudio") == 1)
        {
            EventManager.TriggerEvent(EventTypes.ENABLE_AUDIO);
        }
        else
        {
            EventManager.TriggerEvent(EventTypes.DISABLE_AUDIO);
        } 
    }
}
