using UnityEngine;
using System.Collections.Generic;

public class PlayerIntroAnimController : MonoBehaviour {

    private Animator animator;
    public float duration;
    private bool animationStarted;

    private float counter;


    void Start() {
        animator = GetComponent<Animator>();
        EventManager.StartListening(EventTypes.PLAYER_FLY_IN, OnPlayerFliesIn);
    }

    void OnDestroy() {
        EventManager.StopListening(EventTypes.PLAYER_FLY_IN, OnPlayerFliesIn);
    }

    // Update is called once per frame
    void Update () {
        if (animationStarted) {
            if (counter >= duration) {
                animationStarted = false;
                EventManager.TriggerEvent(EventTypes.PLAYER_IN_POSITION);
                Destroy(animator);
            } else {
                counter += Time.deltaTime;
            }
        }
	}

    void OnPlayerFliesIn(object arg0) {
        animator.SetTrigger("Play");
        animationStarted = true;
    }
}
