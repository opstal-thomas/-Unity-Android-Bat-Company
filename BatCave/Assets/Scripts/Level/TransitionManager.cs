using UnityEngine;
using System.Collections;

public class TransitionManager : MonoBehaviour {
    public EnvironmentModel[] environments;
    private int currentEnviroment = -1;

    [Range(0,100)]
    public float tensionMomentChance;
    public int unlockTensionStage;

    void Start () {
        EventManager.StartListening(EventTypes.CHANGE_ENVIRONMENT, ChangeEnvironment);
	}

    void OnDestroy() {
        EventManager.StopListening(EventTypes.CHANGE_ENVIRONMENT, ChangeEnvironment);
    }

    private void ChangeEnvironment(object value)
    {
        // move to the next stage
        currentEnviroment++;
        int chosenStage = currentEnviroment;

        // check for unlock tension moments
        if (currentEnviroment == unlockTensionStage) {
            EventManager.TriggerEvent(SpawnSystemEvents.UNLOCK_TENSION_MOMENTS);
            EventManager.TriggerEvent(SpawnSystemEvents.START_TENSION_MOMENT);
        } else if (currentEnviroment > unlockTensionStage && Random.Range(0, 100) < tensionMomentChance) {
            EventManager.TriggerEvent(SpawnSystemEvents.START_TENSION_MOMENT);
        } else {
            EventManager.TriggerEvent(SpawnSystemEvents.STOP_TENSION_MOMENT);
        }

        // if all enviroments have been played, start random enviroments
        if (currentEnviroment >= environments.Length)
            chosenStage = Random.Range(0, environments.Length);

        EventManager.TriggerEvent(EventTypes.TRANSITION_END, environments[chosenStage]);
    }
}
