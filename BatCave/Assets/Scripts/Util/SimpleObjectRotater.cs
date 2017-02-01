using UnityEngine;
using System.Collections;

public class SimpleObjectRotater : MonoBehaviour {

    public bool rotate;
    public float turnSpeed;
    public Vector3 axis;

	// Update is called once per frame
	void Update () {
        if (rotate)
            transform.Rotate(axis, turnSpeed * Time.deltaTime);
    }
}
