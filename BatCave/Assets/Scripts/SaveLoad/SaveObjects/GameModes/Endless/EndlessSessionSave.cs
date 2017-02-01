using System;

/// <summary>
/// This class holds information about a single run in endless mode.
/// </summary>
[Serializable]
public class EndlessSessionSave : SaveObject {

    private float resourcesGathered;
    private float score;
    private float echosTimedGood;
    private float echosTimedExcellent;
    private float goodEchoPoint = 100f;
    private float excellentEchoPoint = 200f;
    private int coinsCollected;

    public EndlessSessionSave() {
        
    }

    public void Reset() {
        resourcesGathered = 0f;
        score = 0f;
        echosTimedGood = 0f;
        echosTimedExcellent = 0f;
        coinsCollected = 0;
    }

    public float GetTotalScore() {
        return GetScore() + (GetEchosTimedGood() * GetGoodEchoPoint()) + (GetEchosTimedExcellent() * GetExcellentEchoPoint());
    }

    public float GetGoodEchoPoint() {
        return this.goodEchoPoint;
    }

    public float GetExcellentEchoPoint() {
        return this.excellentEchoPoint;
    }

    public float GetResourcesGathered() {
        return this.resourcesGathered;
    }

    public void AddResourcesGathered(float amount) {
        this.resourcesGathered += amount;
    }

    public float GetScore() {
        return this.score;
    }

    public void SetScore(float value) {
        this.score = value;
    }

    public float GetEchosTimedGood() {
        return this.echosTimedGood;
    }

    public void AddEchosTimedGood(float amount) {
        this.echosTimedGood += amount;
    }

    public float GetEchosTimedExcellent() {
        return this.echosTimedExcellent;
    }

    public void AddEchosTimedExcellent(float amount) {
        this.echosTimedExcellent += amount;
    }

    public int GetTotalCoinsCollected() {
        return this.coinsCollected;
    }

    public void AddCoinsCollected(int amount) {
        this.coinsCollected += amount;
    }
}
