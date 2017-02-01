using UnityEngine;
using System.Collections.Generic;

public class PlayerAnimationController : MonoBehaviour {

    public string BOOST_ANIMATOR_TRIGGER = "Boost";
    public string FLY_ANIMATOR_TRIGGER = "Fly";

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        EventManager.StartListening(PowerupEvents.PLAYER_SPEED_PICKUP, OnSpeedBoostPickedUp);
        EventManager.StartListening(PowerupEvents.PLAYER_SPEED_ENDED, OnSpeedBoostEnded);
    }

    void OnDestroy() {
        EventManager.StopListening(PowerupEvents.PLAYER_SPEED_PICKUP, OnSpeedBoostPickedUp);
        EventManager.StopListening(PowerupEvents.PLAYER_SPEED_ENDED, OnSpeedBoostEnded);
    }

    void OnSpeedBoostPickedUp(object arg0) {
        animator.SetTrigger(BOOST_ANIMATOR_TRIGGER);
    }

    void OnSpeedBoostEnded(object arg0) {
        animator.SetTrigger(FLY_ANIMATOR_TRIGGER);
    }
}
