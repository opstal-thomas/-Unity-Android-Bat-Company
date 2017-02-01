using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupController : MonoBehaviour {

    void Start() {
        EventManager.StartListening(SpawnSystemEvents.ASSIGN_PICKUPS_TO_BATCH, OnBatchRecieved);
    }

    void OnDestroy() {
        EventManager.StopListening(SpawnSystemEvents.ASSIGN_PICKUPS_TO_BATCH, OnBatchRecieved);
    }

    void OnBatchRecieved(object bm) {
        Debug.Log("Start placing pickups");
        BatchModel batchModel = (BatchModel)bm;

        // Get the amount of resource points available for spawning obstacles in this batch
        int resource = batchModel.GetTotalPickupResources();

        // Get a random number for spawnChance
        float spawnChance = Random.Range(0, 100f);

        // Retrieve sub list of obstacles we can spawn given the spawnChance
        List<PickupModel> pickups = batchModel.GetPickupsWithSpawnChance(spawnChance);

        if (pickups.Count != 0) {
            for (int i = 0; i < pickups.Count; i++) {
                if (resource <= 0)
                    break;

                PickupModel pickup = pickups[Random.Range(0, pickups.Count)];

                // if there is room given the laneWeight and if the obstacle is available
                if (resource - pickup.laneWeight >= 0 && pickup.IsAvailable()) {

                    // try to place obstacle in a random slot in the batch
                    bool succesfullyPlaced = batchModel.PlaceGameItemInSpawnPoint(pickup);
                    if (succesfullyPlaced) {
                        resource -= pickup.laneWeight;
                        pickup.SetAvailable(false);
                    }
                }
            }
        }


        // dispatch ready
        EventManager.TriggerEvent(SpawnSystemEvents.PICKUPS_ASSIGNED, batchModel);
    }
}
