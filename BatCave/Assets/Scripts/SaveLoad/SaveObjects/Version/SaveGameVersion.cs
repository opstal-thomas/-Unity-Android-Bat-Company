using System;

[Serializable]
public class SaveGameVersion : SaveObject {
    private const uint SAVE_GAME_VERSION = 1;

    public SaveGameVersion() {
    }

    public uint GetVersion() {
        return SAVE_GAME_VERSION;
    }
}

