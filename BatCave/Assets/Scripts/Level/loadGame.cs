using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void restartGame() {
        SceneManager.LoadScene("scene_One");
    }
}
