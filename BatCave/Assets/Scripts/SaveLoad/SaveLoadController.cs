using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

/// <summary>
/// Manager for saving and loading game data.
/// </summary>
public class SaveLoadController {

    private const string TAG = "SAVELOADCONTROLLER      ";

    private static SaveLoadController instance;

    private List<SaveObject> saveObjects;

    public SaveLoadController() {
        Init();
    }

    private void Init() {
        saveObjects = new List<SaveObject>();

        // init all saveobjects
        OptionsSave os = new OptionsSave();
        PlayerSave ps = new PlayerSave();
        EndlessSessionSave ess = new EndlessSessionSave();
        MultiplayerSessionSave mss = new MultiplayerSessionSave();
        SaveGameVersion sgv = new SaveGameVersion();

        // add to list
        saveObjects.Add(os);
        saveObjects.Add(ps);
        saveObjects.Add(ess);
        saveObjects.Add(mss);
        saveObjects.Add(sgv);
    }

    public static SaveLoadController GetInstance() {
        if (instance == null)
            instance = new SaveLoadController();
        return instance;
    }

    public OptionsSave GetOptions() {
        return (OptionsSave)GetSaveObject(typeof(OptionsSave));
    }

    public PlayerSave GetPlayer() {
        return (PlayerSave)GetSaveObject(typeof(PlayerSave));
    }

    public EndlessSessionSave GetEndlessSession() {
        return (EndlessSessionSave)GetSaveObject(typeof(EndlessSessionSave));
    }

    public MultiplayerSessionSave GetMultiplayerSession() {
        return (MultiplayerSessionSave)GetSaveObject(typeof(MultiplayerSessionSave));
    }

    public SaveGameVersion GetSaveGameVersion() {
        return (SaveGameVersion)GetSaveObject(typeof(SaveGameVersion));
    }

    /// <summary>
    /// Generic function to get a save object from the pool.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private SaveObject GetSaveObject(Type type) {
        foreach (SaveObject obj in saveObjects) {
            if (obj.GetType() == type)
                return obj;
        }
        Debug.LogError("Type passed not found. Is it initilized in the constructor?");
        return null;
    }

    /// <summary>
    /// Converts the list of save objects to an array of bytes used for storing
    /// </summary>
    /// <returns></returns>
    public byte[] CreateSaveObject() {
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream()) {
            bf.Serialize(ms, saveObjects);
            return ms.ToArray();
        }
    }

    /// <summary>
    /// Converts a save file byte array to usable save objects list
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public void RestoreSave(byte[] bytes) {
        try {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Position = 0;
            saveObjects = binForm.Deserialize(memStream) as List<SaveObject>;
        } catch (Exception e) {
            Debug.LogError(TAG + e.StackTrace);
            Debug.Log(TAG + "Error parsing old game data.. resetting save.");
            Init();
        }

        // Check if savegame versions match and migrate data if necessary
        SaveGameVersion currentVersion = new SaveGameVersion();
        if (GetSaveGameVersion() == null) {
            Debug.Log(TAG + "Save game is from before versions were implemented, we can overwrite all and make a clean file.");
            Init();
        } else if (currentVersion.GetVersion() > GetSaveGameVersion().GetVersion()) {
            Debug.Log(TAG + "There is a new savegame version detected. ");

            // For new versions place migration code here...
            // ...
        }
    }
}
