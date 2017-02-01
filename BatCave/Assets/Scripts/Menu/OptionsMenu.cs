using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public Slider movementSensitivitySlider;
    private bool isExiting;

	// Use this for initialization
	void Start () {
        //movementSensitivitySlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        movementSensitivitySlider.value = SaveLoadController.GetInstance().GetOptions().GetControlSensitivity() / 10;
    }
	
	// Update is called once per frame
	void Update () {

        // Android Backbutton is down
        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (!isExiting) {
                isExiting = true;
                // get values
                float value = movementSensitivitySlider.value * 10;
                SaveLoadController.GetInstance().GetOptions().SetControlSensitivity(value);

                // save game
                GooglePlayHelper.GetInstance().SaveGame();

                LoadingController.LoadScene(LoadingController.Scenes.MAIN_MENU);
            }
        }
	}

    //public void OnValueChanged() {
    //    float value = movementSensitivitySlider.value * 10;
    //    SaveLoadController.GetInstance().GetOptions().SetControlSensitivity(value);
    //}

}

