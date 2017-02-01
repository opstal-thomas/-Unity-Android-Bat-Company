using UnityEngine;
using System.Collections;

public class MenuTheme : MonoBehaviour {

    private static MenuTheme instance = null;
    public static MenuTheme Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void StopAudio() {
        Destroy(this.gameObject);
    }
}
