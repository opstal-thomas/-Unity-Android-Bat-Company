using UnityEngine;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour {

    private bool tensionsEnabled;

    void Start() {
        EventManager.StartListening(SpawnSystemEvents.ASSIGN_OBSTACLES_TO_BATCH, OnBatchRecieved);
        EventManager.StartListening(SpawnSystemEvents.START_TENSION_MOMENT, OnTensionMomentStarted);
        EventManager.StartListening(SpawnSystemEvents.STOP_TENSION_MOMENT, OnTensionMomentStopped);
    }

    void OnDestroy() {
        EventManager.StopListening(SpawnSystemEvents.ASSIGN_OBSTACLES_TO_BATCH, OnBatchRecieved);
        EventManager.StopListening(SpawnSystemEvents.UNLOCK_TENSION_MOMENTS, OnTensionMomentStarted);
        EventManager.StopListening(SpawnSystemEvents.STOP_TENSION_MOMENT, OnTensionMomentStopped);
    }

    private void OnTensionMomentStopped(object arg0) {
        tensionsEnabled = false;
    }

    private void OnTensionMomentStarted(object arg0) {
        tensionsEnabled = true;
    }

    void OnBatchRecieved(object bm) {
        BatchModel batchModel = (BatchModel) bm;

        int resource;
        float spawnChance;

        if (tensionsEnabled) {
            resource = 4;
            spawnChance = 100;
        } else {
            // Get the amount of resource points available for spawning obstacles in this batch
            resource = batchModel.GetTotalObstacleResources();

            // Get a random number for spawnChance
            spawnChance = Random.Range(0, 100f);
        }

        // Retrieve sub list of obstacles we can spawn given the spawnChance
        List<ObstacleModel> obstacles = batchModel.GetObstaclesWithSpawnChance(spawnChance);

        if (obstacles.Count != 0) {
            for (int i = 0; i < obstacles.Count; i++) {
                if (resource <= 0)
                    break;

                ObstacleModel obstacle = obstacles[Random.Range(0, obstacles.Count)];

                // if there is room given the laneWeight and if the obstacle is available
                if (resource - obstacle.laneWeight >= 0 && obstacle.IsAvailable()) {
                    
                    // try to place obstacle in a random slot in the batch
                    bool succesfullyPlaced = batchModel.PlaceGameItemInSpawnPoint(obstacle);
                    if (succesfullyPlaced) {
                        resource -= obstacle.laneWeight;
                        obstacle.SetAvailable(false);
                    }
                }
            }
        }

        // dispatch ready
        EventManager.TriggerEvent(SpawnSystemEvents.OBSTACLES_ASSIGNED, batchModel);
    }
}
