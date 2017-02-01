using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

public class GPMPMatchModel : MonoBehaviour {
    public uint minimumAmountOpponents;
    public uint maximumAmountOpponents;
    public Participant player;
    public Participant opponent;
    public bool iAmTheHost;
    public bool playerIsReady;
    public bool opponentIsReady;
}
