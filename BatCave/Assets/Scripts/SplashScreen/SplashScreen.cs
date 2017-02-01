using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

    public float duration;

    private float timer = 0f;
    private bool readyForContinue;

    void Start() {
        if (!Application.isEditor) {
            GooglePlayHelper gph = GooglePlayHelper.GetInstance();
            GooglePlayHelper.GetInstance().ReportEvent(GPGSConstant.event_game_opened, 1);
           
        }
    }

    void Update () {
        timer += Time.deltaTime;
        if (timer >= duration && !readyForContinue) {
            readyForContinue = true;
            LoadingController.LoadScene(LoadingController.Scenes.MAIN_MENU);
        }
	}
}
