using UnityEngine;

public class PowerupController : MonoBehaviour {

    public DayNightCycle dayNightCycle;
    public PlayerControls playerControls;
    public AudioSource audioSource;
    public AudioClip boostLoop;

    bool shieldActive;
    bool speedActive;
    bool lightActive;

    float shieldDuration;
    float speedDuruation;
    float lightDuration;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        EventManager.StartListening(PowerupEvents.PLAYER_SHIELD_PICKUP, OnShieldPickedUp);
        EventManager.StartListening(PowerupEvents.PLAYER_SPEED_PICKUP, OnPlayerSpeedPickedUp);
        EventManager.StartListening(PowerupEvents.PLAYER_LIGHT_PICKUP, OnPlayerLightPickedUp); 
        EventManager.StartListening(PowerupEvents.PLAYER_COIN_PICKUP, OnPlayerCoinPickedUp);
    }

    void Update() {
        if (shieldActive) {
            if (shieldDuration > 0) {
                shieldDuration -= Time.deltaTime;
            } else {
                shieldActive = false;
                if (!speedActive)
                    playerControls.SetShield(false);
                EventManager.TriggerEvent(PowerupEvents.PLAYER_SHIELD_ENDED);
            }
        }

        if (speedActive) {
            if (speedDuruation > 0) {
                speedDuruation -= Time.deltaTime;
            } else {
                speedActive = false;
                dayNightCycle.DecreasePlayerLightRange();
                if (!shieldActive)
                    playerControls.SetShield(false);
                EventManager.TriggerEvent(PowerupEvents.PLAYER_SPEED_ENDED);
                audioSource.loop = false;
                audioSource.Stop();
            }
        }

        if (lightActive) {
            if (lightDuration > 0) {
                lightDuration -= Time.deltaTime;
            } else {
                lightActive = false;
                dayNightCycle.setNightTime();
                EventManager.TriggerEvent(PowerupEvents.PLAYER_LIGHT_ENDED);
            }
        }
    }

    private void OnShieldPickedUp(object duration) {
        shieldActive = true;
        shieldDuration = (float)duration;
        playerControls.SetShield(true);
    }

    private void OnPlayerSpeedPickedUp(object duration) {
        audioSource.clip = boostLoop;
        audioSource.loop = true;
        audioSource.Play();
        speedActive = true;
        speedDuruation = (float) duration;
        playerControls.SetShield(true);
        dayNightCycle.IncreasePlayerLightRange();
    }

    private void OnPlayerLightPickedUp(object lightTime) {
        lightActive = true;
        lightDuration = (float)lightTime;
        dayNightCycle.setDayTime();
    }

    private void OnPlayerCoinPickedUp(object coins) {
        SaveLoadController slc = SaveLoadController.GetInstance();
        slc.GetPlayer().AddTotalCoins((int)coins);
        slc.GetEndlessSession().AddCoinsCollected((int)coins);
    }

    void OnDestroy() {
        EventManager.StopListening(PowerupEvents.PLAYER_SHIELD_PICKUP, OnShieldPickedUp);
        EventManager.StopListening(PowerupEvents.PLAYER_SPEED_PICKUP, OnPlayerSpeedPickedUp);
        EventManager.StopListening(PowerupEvents.PLAYER_LIGHT_PICKUP, OnPlayerLightPickedUp);
        EventManager.StopListening(PowerupEvents.PLAYER_COIN_PICKUP, OnPlayerCoinPickedUp);
    }
}
