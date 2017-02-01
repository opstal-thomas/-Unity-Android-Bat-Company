using GooglePlayGames.BasicApi.Multiplayer;
using System;

/// <summary>
/// This class holds information about a player.
/// </summary>
[Serializable]
public class PlayerSessionSave : SaveObject {

    private string ID;
    private string displayName;

    public PlayerSessionSave(string ID, string displayName) {
        this.ID = ID;
        this.displayName = displayName;
    }

    public string GetID() {
        return this.ID;
    }

    public string GetDisplayName() {
        return this.displayName;
    }
}
