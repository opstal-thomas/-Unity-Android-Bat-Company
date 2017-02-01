using UnityEngine;
using System.Collections;

public class LightDarkControler : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "DarkCollider") {
            EventManager.TriggerEvent(EventTypes.FADE_LIGHT_OUT);
            EventManager.TriggerEvent(EventTypes.SHOW_AND_ENABLE);
            EventManager.TriggerEvent(EventTypes.CHANGE_BACKGROUND);
            Debug.Log("Back to the Darkness!");
        }

        if (col.gameObject.tag == "LightCollider") {
            EventManager.TriggerEvent(EventTypes.FADE_LIGHT_IN);
            EventManager.TriggerEvent(EventTypes.HIDE_AND_DISABLE);
            EventManager.TriggerEvent(EventTypes.CHANGE_BACKGROUND);
            Debug.Log("It's going to get Bright!");
        }
    }
}
