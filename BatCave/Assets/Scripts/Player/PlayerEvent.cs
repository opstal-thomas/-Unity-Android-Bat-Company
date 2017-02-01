using UnityEngine;
using System.Collections.Generic;

public class PlayerEvent : MonoBehaviour {
    public PlayerResources playerResource;
    public PlayerControls playerControls;

    void OnEnable() {
        EventManager.StartListening(EventTypes.HEALTH_PICKED_UP, healthPickedUp);
        EventManager.StartListening(EventTypes.ECHO_USED_RESOURCES, echoUsed);
        EventManager.StartListening(EventTypes.GOOD_ECHO, GoodEcho);
        EventManager.StartListening(EventTypes.PERFECT_ECHO, PerfectEcho);
    }

    void OnDisable() {
        EventManager.StopListening(EventTypes.HEALTH_PICKED_UP, healthPickedUp);
        EventManager.StopListening(EventTypes.ECHO_USED_RESOURCES, echoUsed);
        EventManager.StopListening(EventTypes.GOOD_ECHO, GoodEcho);
        EventManager.StopListening(EventTypes.PERFECT_ECHO, PerfectEcho);
    }

    private void healthPickedUp(object arg0) {
        playerResource.addHealth(playerResource.healthPickupAmount);
        SaveLoadController.GetInstance().GetEndlessSession().AddResourcesGathered(1);
    }

    private void echoUsed(object arg0) {
        playerResource.echoUsed();
    }

    private void PerfectEcho(object value) {
        playerResource.GoodEcho();
    }

    private void GoodEcho(object value) {
        playerResource.PerfectEcho();
    }
}
