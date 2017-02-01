using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// LoadingController functions as a simple bridge between scenes.
/// </summary>
/// <remarks>
/// Call the static method 'LoadScene' from anywhere to start loading a scene.
/// The scene will automatically launch when done loading.
/// </remarks>
public class LoadingController : MonoBehaviour {

    /// <summary>
    /// These enums represent scenes that can be loaded. The value 
    /// of each enum is a reference to the index of the scene.
    /// See unity build settings for the correct indexes.
    /// </summary>
    public enum Scenes {
        SPLASH_SCREEN = 0,
        MAIN_MENU = 1,
        GAME = 2,
        GAME_OVER = 3,
        LOADING_SCENE = 4,
        INFO_MENU = 5,
        GPMP_LOBBY = 6,
        GPMP_GAME = 7,
        GPMP_WAITING_ROOM = 8,
        GPMP_VERSUS_SCREEN = 9,
        GPMP_GAME_OVER = 10,
        STORE = 11
    }

    private static Scenes sceneToBeLoaded;
    private AsyncOperation async;

    public Text loadingPercentageTextField;

    public static void LoadScene(Scenes scene) {
        sceneToBeLoaded = scene;
        SceneManager.LoadScene((int)Scenes.LOADING_SCENE);
    }

    void Start() {
        StartCoroutine(LoadGame());
    }

    void Update() {
        if (async != null) {
            loadingPercentageTextField.text = (Math.Ceiling(async.progress * 100)).ToString() + "%";
            if (async.progress >= 0.9f) {
                Debug.Log("done loading");
                async.allowSceneActivation = true;
            }
        }
    }


    private IEnumerator LoadGame() {
        async = SceneManager.LoadSceneAsync((int)sceneToBeLoaded);
        async.allowSceneActivation = false;
        Debug.Log("start loading");
        yield return async;
    }
}
