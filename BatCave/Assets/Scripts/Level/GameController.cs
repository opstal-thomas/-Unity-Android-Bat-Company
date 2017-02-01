using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject UILeft;
    public GameObject skillSlider;
    public GameObject HealtBar;
    public GameObject ResourceBar;
    public GameObject ResourceSlider;
    public GameObject echoBar;

    public ScoreCalculator scoreCalculator;
    public Light directionalLight;
    public float fadeOutDelay;
    public float fadeOutSpeed;
    public float playerFliesInDelay;
    public int scoreIntervalTension;
    public int scoreForPowerups;

    public float playerDiesTime;

    private float playerDiesCounter;
    private float fadeOutDelayCounter;
    private float playerFliesInCounter;
    private bool playerDied;
    private bool playerFlyInTriggered;
    private bool powerUpsActive = false;

    // Use this for initialization
    void Start() {
        EventManager.StartListening(EventTypes.GAME_OVER, OnGameOver);
        EventManager.StartListening(EventTypes.GAME_START, OnGameStart);
        EventManager.StartListening(EventTypes.PLAYER_DIED, OnPlayerDied);
        EventManager.StartListening(EventTypes.PLAYER_IN_POSITION, OnPlayerPositioned);

        EventManager.TriggerEvent(EventTypes.GAME_START);
    }

    void FixedUpdate() {
        if (directionalLight != null) {
            if (fadeOutDelayCounter >= fadeOutDelay) {
                if (!playerFlyInTriggered) {
                    EventManager.TriggerEvent(EventTypes.ENABLE_PLAYER_LIGHT);
                    playerFlyInTriggered = true;
                }
                directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, 0f, fadeOutSpeed * Time.deltaTime);
                if (directionalLight.intensity <= 0.5f) {
                    Destroy(directionalLight.gameObject);
                }
            } else {
                fadeOutDelayCounter += Time.deltaTime;
            }
        }
        if (playerFliesInCounter != -1f) {
            if (playerFliesInCounter >= playerFliesInDelay) {
                playerFliesInCounter = -1f;
                EventManager.TriggerEvent(EventTypes.PLAYER_FLY_IN);
            } else {
                playerFliesInCounter += Time.deltaTime;
            }
        }
    }

    void Update() {
        if (playerDied) {
            playerDiesCounter += Time.deltaTime;
            if (playerDiesCounter >= playerDiesTime) {
                playerDied = false;
                EventManager.TriggerEvent(EventTypes.GAME_OVER);
            }
        }
    }

    // called when the intro animation has finished
    void OnPlayerPositioned(object arg0) {
        transform.position = new Vector3(0,0,transform.position.z);
        Destroy(GetComponent<Animator>());
        //EventManager.TriggerEvent(SpawnSystemEvents.TOGGLE_SPAWNING, true);

        // Reactivate UI
        //skillSlider.SetActive(true);
        pauseButton.SetActive(true);
        UILeft.SetActive(true);
        //skillSlider.SetActive(true);
        HealtBar.SetActive(true);
        ResourceBar.SetActive(true);
        ResourceSlider.SetActive(true);
        echoBar.SetActive(true);
}

    void OnPlayerDied(object arg0) {
        playerDied = true;
        pauseButton.SetActive(false);
    }

    void OnGameStart(object arg0) {
        // reset player model
        SaveLoadController.GetInstance().GetEndlessSession().Reset();

        // hide UI
        pauseButton.SetActive(false);
        UILeft.SetActive(false);
        skillSlider.SetActive(false);
        HealtBar.SetActive(false);
        ResourceBar.SetActive(false);
        ResourceSlider.SetActive(false);
        echoBar.SetActive(false);
    }

    public void PauseGame() {
        EventManager.TriggerEvent(EventTypes.GAME_PAUSED);
        pausePanel.SetActive(true);
    }

    public void ResumeGame() {
        pausePanel.SetActive(false);
        EventManager.TriggerEvent(EventTypes.GAME_RESUME);
    }

    void OnGameOver(object arg0) {
        PlayerSave player = SaveLoadController.GetInstance().GetPlayer();
        EndlessSessionSave gameSession = SaveLoadController.GetInstance().GetEndlessSession();
        GooglePlayHelper gph = GooglePlayHelper.GetInstance();
        player.AddTotalGamesPlayed(1);

        // report events
        gph.ReportEvent(GPGSConstant.event_amount_of_endless_games_started, 1);
        gph.ReportEvent(GPGSConstant.event_score_endless_mode, gameSession.GetTotalScore());
        gph.ReportEvent(GPGSConstant.event_health_potions_picked_up, gameSession.GetResourcesGathered());

        // save current stats
        gph.SaveGame(); // TODO: keep track of timeplayed

        // check for achievements
        AchievementChecker.CheckForEndlessScoreAchievement(gameSession.GetTotalScore());
        AchievementChecker.CheckForWelcomeAchievement();

        // highscore post
        if (gameSession.GetTotalScore() > player.GetHighscore()) {
            EventManager.TriggerEvent(EventTypes.NEW_HIGHSCORE);
            player.SetHighscore(gameSession.GetTotalScore());
            gph.PostHighscore(player.GetHighscore(), GPGSConstant.leaderboard_endless_mode);
        }
        
        // start game over screen
        LoadingController.LoadScene(LoadingController.Scenes.GAME_OVER);

    }

    public void MainMenu()
    {
        LoadingController.LoadScene(LoadingController.Scenes.MAIN_MENU);
    }

    void OnDestroy() {
        EventManager.StopListening(EventTypes.GAME_OVER, OnGameOver);
        EventManager.StopListening(EventTypes.GAME_START, OnGameStart);
        EventManager.StopListening(EventTypes.PLAYER_DIED, OnPlayerDied);
        EventManager.StopListening(EventTypes.PLAYER_IN_POSITION, OnPlayerPositioned);
    }
}
