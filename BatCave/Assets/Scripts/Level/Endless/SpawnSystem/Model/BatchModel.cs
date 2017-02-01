using System.Collections.Generic;
using UnityEngine;

public class BatchModel {

    private SpawnPointModel[] spawnPoints;
    private PickupModel[] pickups; // contains all pickups in the game
    private ObstacleModel[] obstacles; // contains all obstacles in the game

    private int totalObstacleResources;
    private int totalPickupResouces;
    private float maxYoffset;

    public BatchModel(SpawnPointModel[] spawnPoints, PickupModel[] pickups, ObstacleModel[] obstacles, int totalObstacleResources, int totalPickupResouces, float maxYoffset) {
        this.spawnPoints = spawnPoints;
        this.pickups = pickups;
        this.obstacles = obstacles;
        this.totalObstacleResources = totalObstacleResources;
        this.totalPickupResouces = totalPickupResouces;
        this.maxYoffset = maxYoffset;
    }

    public SpawnPointModel[] GetSpawnPoints() {
        return this.spawnPoints;
    }

    public PickupModel[] GetPickups() {
        return this.pickups;
    }

    public ObstacleModel[] GetObstacles() {
        return this.obstacles;
    }

    public List<ObstacleModel> GetObstaclesWithSpawnChance(float spawnChance) {
        List<ObstacleModel> result = new List<ObstacleModel>();
        foreach (ObstacleModel om in obstacles) {
            if (spawnChance <= om.spawnChance)
                result.Add(om);
        }
        return result;
    }

    public List<PickupModel> GetPickupsWithSpawnChance(float spawnChance) {
        List<PickupModel> result = new List<PickupModel>();
        foreach (PickupModel pm in pickups) {
            if (spawnChance <= pm.spawnChance)
                result.Add(pm);
        }
        return result;
    }

    public int GetTotalObstacleResources() {
        return this.totalObstacleResources;
    }

    public int GetTotalPickupResources() {
        return this.totalPickupResouces;
    }

    public float GetMaxYoffset() {
        return this.maxYoffset;
    }

    public void ResetSpawnPoints() {
        foreach (SpawnPointModel sp in spawnPoints) {
            sp.Reset();
        }
    }

    public bool PlaceGameItemInSpawnPoint(GameItem item) {
        List<SpawnPointModel> spawnPointOptions = new List<SpawnPointModel>();

        // find possible solutions to place the obstacle
        for (int i = 0; i < spawnPoints.Length - item.laneWeight + 1; i++) {
            if (spawnPoints[i].IsSlotTaken())
                continue;

            // check if the obstacle can fit in the current slot
            bool canSpawn = true;
            for (int weight = 1; weight < item.laneWeight + 1; weight++) {
                if (spawnPoints[i + weight - 1].IsSlotTaken())
                    canSpawn = false;
            }
            if (canSpawn) {
                spawnPointOptions.Add(spawnPoints[i]);
            }
        }

        if (spawnPointOptions.Count > 0) {
            // place obstacle in a random slot
            SpawnPointModel randomOption = spawnPointOptions[Random.Range(0, spawnPointOptions.Count)];

            // reserve spawnpoint slots if an object is bigger than one lane
            for (int i = 0; i < spawnPoints.Length; i++) {
                if (spawnPoints[i] == randomOption) {
                    for (int weight = 1; weight < item.laneWeight + 1; weight++) {
                        spawnPoints[i + weight - 1].reserveSlot();
                    }
                    break;
                }
            }
            
            randomOption.SetItem(item);
            return true;
        } else {
            return false;
        }
    }
}
