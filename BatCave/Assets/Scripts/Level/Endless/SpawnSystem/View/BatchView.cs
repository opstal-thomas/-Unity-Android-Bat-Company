using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BatchView : MonoBehaviour {

    public bool isNetwork;
    private float networkScore;

    public SpawnPointModel[] spawnPoints;

    public PickupModel[] pickups;
    private PickupModel[] pickupsForStage;

    public ObstacleModel[] obstacles;
    private ObstacleModel[] obstaclesForStage;

    public GameObject cleanUp;
    public ScoreCalculator scoreCalculator;

    public float increaseDifficultyStep;
    public float highestDifficultyScore;
    public float stageDurationInScore;

    public float initialSpawnDelay;
    public float endSpawnDelay;
    public float currentSpawnDelay;
    public float spawnDelay;

    public int initialTotalObstacleResources;
    public int endTotalObstacleResources;
    public int currentTotalObstacleResources;

    public int initialTotalPickupResouces;
    public int endTotalPickupResouces;
    public int currentTotalPickupResources;

    public float initialYoffsetError;
    public float endYoffsetError;
    public float currentYoffsetError;

    public int afterWhatStageBooster;
    public GameObject SpeedBooster;

    private int currentStage = -1;

    void Start() {
        EventManager.StartListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StartListening(EventTypes.GAME_RESUME, OnGameResumed);
        EventManager.StartListening(EventTypes.CHANGE_ENVIRONMENT, OnEnviromentChanged);
        EventManager.StartListening(SpawnSystemEvents.TOGGLE_SPAWNING, OnSpawningToggled);

        // GPMP
        EventManager.StartListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnGameStartReady);
    }



    void OnEnviromentChanged(object type) {
        currentStage++;

        Debug.Log("Current stage: " + currentStage);

        // select only available pickups and obstacles
        List<PickupModel> pickupList = new List<PickupModel>();
        foreach (PickupModel pm in pickups) {
            if (currentStage >= pm.GetStageLevel()) {
                pickupList.Add(pm);
                Debug.Log("Adding pickup: " + pm.gameObject.name);
            }
        }

        List<ObstacleModel> obstacleList = new List<ObstacleModel>();
        foreach (ObstacleModel om in obstacles) {
            if (currentStage >= om.GetStageLevel()) {
                obstacleList.Add(om);
            }
        }

        pickupsForStage = pickupList.ToArray();
        obstaclesForStage = obstacleList.ToArray();

        StartCoroutine(StartSpawnDelay()); //start spawning
    }

    IEnumerator StartSpawnDelay() {
        yield return new WaitForSeconds(spawnDelay);
        EventManager.TriggerEvent(SpawnSystemEvents.TOGGLE_SPAWNING, true);
    }

    void OnGameResumed(object arg0) {
        OnSpawningToggled(true);
    }

    void OnGamePaused(object arg0) {
        OnSpawningToggled(false);
    }

    void OnDestroy() {
        EventManager.StopListening(SpawnSystemEvents.TOGGLE_SPAWNING, OnSpawningToggled);
        EventManager.StopListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StopListening(EventTypes.GAME_RESUME, OnGameResumed);
        EventManager.StopListening(EventTypes.CHANGE_ENVIRONMENT, OnEnviromentChanged);

        // GPMP
        EventManager.StopListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnGameStartReady);
    }

    float UpdateCurrentValueByScore(float currentDifficultyStep, float totalDifficultySteps , float initValue, float endValue) {
        float highest = Math.Max(initValue, endValue); 
        float lowest = Math.Min(initValue, endValue); 
        float a = (highest - lowest) / totalDifficultySteps; 
        if (endValue > initValue) {
            return initValue + (a * currentDifficultyStep);
        } else {
            return initValue - (a * currentDifficultyStep);
        }
    }

    void FixedUpdate() {
        float score;
        if (isNetwork) {
            networkScore++;
            score = networkScore;
        } else {
            score = scoreCalculator.playerScore;
        }

        if (score <= highestDifficultyScore && score != 0) {

            // check if difficulty needs to go up
            if (score % increaseDifficultyStep == 0) {
                // increase difficulty
                float currentStep = score / increaseDifficultyStep;
                float totalDifficultySteps = highestDifficultyScore / increaseDifficultyStep;

                currentSpawnDelay = UpdateCurrentValueByScore(currentStep, totalDifficultySteps, initialSpawnDelay, endSpawnDelay);
                currentTotalObstacleResources = (int)UpdateCurrentValueByScore(currentStep, totalDifficultySteps, initialTotalObstacleResources, endTotalObstacleResources);
                currentTotalPickupResources = (int)UpdateCurrentValueByScore(currentStep, totalDifficultySteps, initialTotalPickupResouces, endTotalPickupResouces);
                currentYoffsetError = UpdateCurrentValueByScore(currentStep, totalDifficultySteps, initialYoffsetError, endYoffsetError);
            }
        }

        if (score % stageDurationInScore == 0 && score != 0 && !isNetwork) {
            EventManager.TriggerEvent(EventTypes.TRANSITION_START);
            EventManager.TriggerEvent(SpawnSystemEvents.TOGGLE_SPAWNING, false);
        }
        
        if (score == (stageDurationInScore*afterWhatStageBooster) && !isNetwork) {
            SpeedBooster.transform.position = new Vector3(0, 35, 0); //hacky fix to get booster to spawn at the correct time
            SpeedBooster.GetComponent<GameItem>().SetAvailable(false);
        }

        if (score == 100 && !isNetwork) {
            EventManager.TriggerEvent(EventTypes.CHANGE_ENVIRONMENT);
        }
    }
    
    private void OnSpawningToggled(object arg0) {
        bool enabled = (bool)arg0;
        if (!cleanUp.activeInHierarchy)
            cleanUp.SetActive(true);
        if (enabled)
            StartCoroutine("CreateNewBatch");
        else
            StopCoroutine("CreateNewBatch");
    }

    IEnumerator CreateNewBatch() {
        while (true) {
            Debug.Log("StartSpawning");

            // Create new batch model
            BatchModel batchModel;
            batchModel = new BatchModel(spawnPoints, pickupsForStage, obstaclesForStage, currentTotalObstacleResources, currentTotalPickupResources, currentYoffsetError);

            if (isNetwork) {
                batchModel = new BatchModel(spawnPoints, pickups, obstacles, currentTotalObstacleResources, currentTotalPickupResources, currentYoffsetError);
            }

            EventManager.TriggerEvent(SpawnSystemEvents.NEW_BATCH_CREATED, batchModel);
            
            yield return new WaitForSeconds(currentSpawnDelay);
        }
    }

    // GPMP
    private void OnGameStartReady(object matchModel) {
        GPMPMatchModel model = (GPMPMatchModel)matchModel;
        DebugMP.Log("Game start revieved by batch view. I am host: " + model.iAmTheHost);
        if (model.iAmTheHost)
            EventManager.TriggerEvent(SpawnSystemEvents.TOGGLE_SPAWNING, true);
    }
}
