using UnityEngine;
using System.Collections;

public class SimpleDestroyScript : MonoBehaviour {

    public float destroyDelayInSeconds;
    public GameObject target;

    private float destroyDelayCounter;
    
	// Update is called once per frame
	void Update () {
        destroyDelayCounter += Time.deltaTime;
        if (destroyDelayCounter >= destroyDelayInSeconds) {
            Destroy(target);
        }
    }
}
