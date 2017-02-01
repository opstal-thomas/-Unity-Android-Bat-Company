using UnityEngine;

public class InfoScene : MonoBehaviour {

    public void GoToMenu() {
        LoadingController.LoadScene(LoadingController.Scenes.MAIN_MENU);
    }
}
