using UnityEngine;
using System.Collections;

public class DebugMP : MonoBehaviour {

    private const string GPMP_LOG_MESSAGE = "GPMP_LOG_MESSAGE   ";

    public static void Log(string message) {
        Debug.Log(GPMP_LOG_MESSAGE + message);
    }
}
