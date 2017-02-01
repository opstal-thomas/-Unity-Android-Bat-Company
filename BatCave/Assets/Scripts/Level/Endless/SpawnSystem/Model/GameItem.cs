using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class GameItem : MonoBehaviour {

    [Range(0f, 100f)]
    public float spawnChance;

    [Range(1, 5)]
    public int laneWeight;

    public int stageLevel;

    public bool changeSpriteWithEnviroment;
    public Sprite woods;
    public Sprite purpleCave;
    public Sprite iceCave;
    public Sprite hell;

    // GPMP
    // This ID must be unique for every gameitem in the scene
    public uint networkID;

    private bool isAvailable = true;
    private SpriteRenderer spriteRenderer;
    private Sprite nextSprite;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.StartListening(EventTypes.TRANSITION_START, OnTransitionStart);
        EventManager.StartListening(EventTypes.TRANSITION_END, OnTransitionEnd);
        EventManager.StartListening(GPMPEvents.Types.GPMP_GAME_ITEM_RECIEVED.ToString(), OnGameItemRecieved);
    }

    void Update() {
        if (isAvailable && nextSprite != null) {
            spriteRenderer.sprite = nextSprite;
            nextSprite = null;
        }
    }

    void OnTransitionEnd(object type) {
        if (!changeSpriteWithEnviroment)
            return;
        Sprite sprite;
        string enviroment = type.ToString();
        switch (enviroment) {
            case EnvironmentTypes.WOODS:
                sprite = woods;
                break;
            case EnvironmentTypes.PURPLE_CAVE:
                sprite = purpleCave;
                break;
            case EnvironmentTypes.ICE_CAVE:
                sprite = iceCave;
                break;
            case EnvironmentTypes.HELL:
                sprite = hell;
                break;
            default:
                Debug.LogError("Enviroment type " + enviroment + " not found!");
                return;
        }

        SetSprite(sprite);
    }

    void OnTransitionStart(object arg0) {
        if (changeSpriteWithEnviroment)
            SetSprite(woods);
    }

    void SetSprite(Sprite sprite) {
        if (isAvailable) {
            spriteRenderer.sprite = sprite;
        } else {
            nextSprite = sprite;
        }
    }

    void OnDestroy() {
        EventManager.StopListening(EventTypes.TRANSITION_START, OnTransitionStart);
        EventManager.StopListening(EventTypes.TRANSITION_END, OnTransitionEnd);
        EventManager.StopListening(GPMPEvents.Types.GPMP_GAME_ITEM_RECIEVED.ToString(), OnGameItemRecieved);
    }

    public bool IsAvailable() {
        return this.isAvailable;
    }

    public void SetAvailable(bool available) {
        this.isAvailable = available;
    }

    public int GetStageLevel() {
        return this.stageLevel;
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "CleanUp") {
            SetAvailable(true);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "CleanUp") {
            SetAvailable(true);
        }
    }

    // GPMP
    public List<byte> ToBytes() {
        //DebugMP.Log("Transforming obstacle to bytes");
        List<byte> bytes = new List<byte>();
        Vector3 pos = gameObject.transform.position;
        bytes.AddRange(BitConverter.GetBytes(networkID));
        bytes.AddRange(BitConverter.GetBytes(pos.x));
        bytes.AddRange(BitConverter.GetBytes(pos.y));
        bytes.AddRange(BitConverter.GetBytes(pos.z));
        return bytes;
    }

    // GPMP
    private void OnGameItemRecieved(object b) {
        //DebugMP.Log("Recieved obstacle from host type: " + b.GetType());
        //List<byte> arrayList = (List<byte>)b;
        byte[] bytes = (byte[])b;

        uint ID = BitConverter.ToUInt32(bytes, 5);

        //DebugMP.Log("recieved ID: " + ID + "\t" + "my ID: " + networkID);
        if (ID == networkID) {
            //DebugMP.Log("Match found. setting position now");
            
            gameObject.transform.position = new Vector3(BitConverter.ToSingle(bytes, 9), BitConverter.ToSingle(bytes, 13), BitConverter.ToSingle(bytes, 17));
        }
    }

}
