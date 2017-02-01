using UnityEngine;
using System.Collections;
using System;
using System.Net;

/// <summary>
/// Code for checking network status from:
/// https://docs.unity3d.com/ScriptReference/Network.TestConnection.html
/// </summary>
public class InternetConnectionStatus : MonoBehaviour {

    private const string TAG = "INTERNET_CONNECTION_STATUS  ";

    public const string TEST_CONNECTION_STATUS = "InternetConnectionStatus_TEST_CONNECTION_STATUS";
    public const string CONNECTION_STATUS_UPDATE = "InternetConnectionStatus_CONNECTION_STATUS_UPDATE";
    public const string SHOW_CONNECTION_STATE = "InternetConnectionStatus_SHOW_CONNECTION_STATE";

    string goodConnection = "Internet connection available.";
    string limitedConnection = "Your current internet connection is limited. You might experience some troubles playing online.";
    string noConnection = "You are not connected to the internet. Online features are not available.";

    string testStatus = "Testing network connection capabilities.";
    string testMessage = "Test in progress";
    string shouldEnableNatMessage = "";
    bool doneTesting = true;
    bool startTesting = false;
    bool probingPublicIP = false;
    int serverPort = 9999;

    ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;
    private Status connectionStatus = Status.UNKNOWN;
    private float timer;
    private bool useNat;

    public enum Status {
        CONNECTED,
        LIMITED,
        NO_CONNECTION,
        UNKNOWN
    }

    void Start() {
        EventManager.StartListening(TEST_CONNECTION_STATUS, OnConnectionStatusRequest);
    }

    void OnDestroy() {
        EventManager.StopListening(TEST_CONNECTION_STATUS, OnConnectionStatusRequest);
    }

    private void OnConnectionStatusRequest(object arg0) {
        TestConnection();
    }

    public string GetCurrentConnectionStatusInfo() {
        return this.testMessage;
    }

    void Update() {
        if (!doneTesting)
            TestConnectionNAT();
    }

    void TestConnection() {
        string externalip = "";
        try {
            externalip = new WebClient().DownloadString("http://icanhazip.com");
        } catch (Exception e) {
            externalip = "";
        }

        if (Network.HavePublicAddress() || externalip != "") {
            doneTesting = false;
            connectionTestResult = ConnectionTesterStatus.Undetermined;
        } else {
            connectionStatus = Status.NO_CONNECTION;
            testMessage = noConnection;
            EventManager.TriggerEvent(CONNECTION_STATUS_UPDATE, connectionStatus);
            Debug.Log(TAG + connectionStatus + "\t" + "Status: No public IP");
        }
    }

    void TestConnectionNAT() {
        connectionTestResult = Network.TestConnectionNAT();
        switch (connectionTestResult) {
            case ConnectionTesterStatus.Error:
                testMessage = noConnection;
                doneTesting = true;
                connectionStatus = Status.NO_CONNECTION;
                break;

            case ConnectionTesterStatus.Undetermined:
                testMessage = noConnection;
                doneTesting = false;
                connectionStatus = Status.UNKNOWN;
                break;

            case ConnectionTesterStatus.PublicIPIsConnectable:
                testMessage = goodConnection;
                useNat = false;
                doneTesting = true;
                connectionStatus = Status.CONNECTED;
                break;

            // This case is a bit special as we now need to check if we can 
            // circumvent the blocking by using NAT punchthrough
            case ConnectionTesterStatus.PublicIPPortBlocked:
                testMessage = limitedConnection;
                useNat = false;
                // If no NAT punchthrough test has been performed on this public 
                // IP, force a test
                if (!probingPublicIP) {
                    connectionTestResult = Network.TestConnectionNAT();
                    probingPublicIP = true;
                    timer = Time.time + 10;
                }
                // NAT punchthrough test was performed but we still get blocked
                else if (Time.time > timer) {
                    probingPublicIP = false;        // reset
                    useNat = true;
                    doneTesting = true;
                    connectionStatus = Status.LIMITED;
                }
                break;

            case ConnectionTesterStatus.PublicIPNoServerStarted:
                testMessage = goodConnection;
                useNat = true;
                doneTesting = true;
                connectionStatus = Status.CONNECTED;
                break;

            case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
                testMessage = limitedConnection;
                useNat = true;
                doneTesting = true;
                connectionStatus = Status.LIMITED;
                break;

            case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
                testMessage = limitedConnection;
                useNat = true;
                doneTesting = true;
                connectionStatus = Status.LIMITED;
                break;

            case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
            case ConnectionTesterStatus.NATpunchthroughFullCone:
                testMessage = limitedConnection;
                useNat = true;
                doneTesting = true;
                connectionStatus = Status.LIMITED;
                break;

            default:
                testMessage = noConnection;
                connectionStatus = Status.NO_CONNECTION;
                break;
        }

        if (doneTesting) {
            EventManager.TriggerEvent(CONNECTION_STATUS_UPDATE, connectionStatus);
            Debug.Log(TAG + connectionStatus + "\t" + "Status: " + connectionTestResult);
        }
    }

}
