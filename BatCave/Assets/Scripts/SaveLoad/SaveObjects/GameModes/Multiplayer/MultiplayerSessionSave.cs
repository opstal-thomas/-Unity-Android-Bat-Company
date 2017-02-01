using GooglePlayGames.BasicApi.Multiplayer;
using System;

/// <summary>
/// This class holds information about a single run in multiplayer mode.
/// </summary>
[Serializable]
public class MultiplayerSessionSave : SaveObject {

    private PlayerSessionSave player;
    private PlayerSessionSave opponent;
    private PlayerSessionSave winningPlayer;

    public MultiplayerSessionSave() {

    }

    public void Reset() {
        winningPlayer = null;
        player = null;
        opponent = null;
    }

    public void SetPlayerWon() {
        this.winningPlayer = this.player;
    }

    public void SetOpponentWon() {
        this.winningPlayer = this.opponent;
    }

    public PlayerSessionSave GetPlayer() {
        return this.player;
    }

    public PlayerSessionSave GetOpponent() {
        return this.opponent;
    }

    public void SetPlayers(Participant player, Participant opponent) {
        this.player = new PlayerSessionSave(player.ParticipantId, player.DisplayName);
        this.opponent = new PlayerSessionSave(opponent.ParticipantId, opponent.DisplayName);
    }

    public PlayerSessionSave GetWinningPlayer() {
        return this.winningPlayer;
    }

    public void SetWinningPlayer(PlayerSessionSave p) {
        this.winningPlayer = p;
    }
    
}
