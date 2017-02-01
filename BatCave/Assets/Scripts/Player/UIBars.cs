using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBars : MonoBehaviour {
    public PlayerControls player;
    public PlayerResources playerResources;

    //health
    public float healthPercentage;
    public Slider healthBar;

    //Special Power
    public Slider specialBar;
    public float specialPercentage;
	
	// Update is called once per frame
	void Update () {
        UpdateResourceBar();
        UpdateSpecialBar();
    }

    private void UpdateSpecialBar() {
        specialPercentage = playerResources.echoComboAmount / playerResources.maxEchoComboAmount;
        specialBar.value = specialPercentage;
    }

    void UpdateResourceBar() {
        healthPercentage = playerResources.health / playerResources.maxHealth;
        healthBar.value = healthPercentage;
    }

    public void ActivteShapeShift() {
        EventManager.TriggerEvent(EventTypes.SHAPE_SHIFT);
    }
}
