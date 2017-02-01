using UnityEngine;
using System.Collections;

public class PlayerResources : MonoBehaviour {
    public float health;
    public float maxHealth;
    public int echoUsedAmount;

    public int damageAmount;
    public int healthPickupAmount;

    //Special resource
    public float maxEchoComboAmount;
    public float echoComboAmount;
    public float perfectEchoReward;
    public float goodEchoReward;
    public float minusSpecialResources;

    private bool swipeUp;

    private void Start() {
        EventManager.StartListening(EventTypes.SPECIAL_USED, SpecialUsed);
        EventManager.StartListening(EventTypes.PLAYER_TAKES_DAMAGE, PlayerHit);
        swipeUp = true;
    }

    private void OnDestroy() {
        EventManager.StopListening(EventTypes.SPECIAL_USED, SpecialUsed);
        EventManager.StopListening(EventTypes.PLAYER_TAKES_DAMAGE, PlayerHit);
    }

	// Update is called once per frame
	void Update () {
        if (health < 0) {
            health = 0;
        }

        if (health > maxHealth) {
            health = maxHealth;
        }

        if (echoComboAmount > maxEchoComboAmount) {
            echoComboAmount = maxEchoComboAmount;
        }

        if (echoComboAmount < 0) {
            echoComboAmount = 0;
        }

        if (echoComboAmount == maxEchoComboAmount && swipeUp) {
            EventManager.TriggerEvent(EventTypes.SWIPE_UP);
            swipeUp = false;
        }
	}

    public void addHealth(int value) {
        health += value;
    }

    public void removeHealth(int value) {
        health -= value;
    }

    public void echoUsed() {
        echoUsedAmount += 1;
    }

    public void GoodEcho() {
        echoComboAmount += goodEchoReward;
    }

    public void PerfectEcho() {
        echoComboAmount += perfectEchoReward;
    }

    private void SpecialUsed(object value) {
        echoComboAmount -= maxEchoComboAmount;
        swipeUp = true;
    }

    private void PlayerHit(object value)
    {
        if (echoComboAmount == maxEchoComboAmount) {
            EventManager.TriggerEvent(EventTypes.CANCEL_SWIPE_UP);
            swipeUp = true;
        }
        echoComboAmount -= minusSpecialResources;
    }
}
