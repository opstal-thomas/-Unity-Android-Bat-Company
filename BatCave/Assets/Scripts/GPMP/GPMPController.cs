using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;

public class GPMPController : MonoBehaviour, RealTimeMultiplayerListener {
    
    public static bool playerLeftOnPurpose = true;
    public static string errorMessage;
    
    void Start() {
        EventManager.StartListening(GPMPEvents.Types.GPMP_CANCEL_MATCH_MAKING.ToString(), OnMatchMakingCanceled);
        EventManager.StartListening(GPMPEvents.Types.GPMP_LEAVE_GAME.ToString(), OnPlayerLeavesTheGame); 
        EventManager.StartListening(GPMPEvents.Types.GPMP_VIEW_INVITES.ToString(), OnViewInvites);
        EventManager.StartListening(GPMPEvents.Types.GPMP_SEARCH_QUICK_MATCH.ToString(), OnStartSearchForQuickMatch);
        EventManager.StartListening(GPMPEvents.Types.GPMP_START_WITH_INVITE.ToString(), OnStartInvitingForMatch);
    }

    void OnDestroy() {
        EventManager.StopListening(GPMPEvents.Types.GPMP_CANCEL_MATCH_MAKING.ToString(), OnMatchMakingCanceled);
        EventManager.StopListening(GPMPEvents.Types.GPMP_LEAVE_GAME.ToString(), OnPlayerLeavesTheGame);
        EventManager.StopListening(GPMPEvents.Types.GPMP_VIEW_INVITES.ToString(), OnViewInvites);
        EventManager.StopListening(GPMPEvents.Types.GPMP_SEARCH_QUICK_MATCH.ToString(), OnStartSearchForQuickMatch);
        EventManager.StopListening(GPMPEvents.Types.GPMP_START_WITH_INVITE.ToString(), OnStartInvitingForMatch);
    }

    private static GPMPController instance;

    public static GPMPController GetInstance() {
        if (instance == null)
            instance = new GPMPController();
        return instance;
    }

    void OnViewInvites(object obj) {
        PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(this);
    }

    void OnPlayerLeavesTheGame(object arg0) {
        DebugMP.Log("OnPlayerLeavesTheGame");
        playerLeftOnPurpose = true;
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        LoadingController.LoadScene(LoadingController.Scenes.GPMP_LOBBY);
    }

    public void OnMatchMakingCanceled(object obj) {
        DebugMP.Log("OnMatchMakingCanceled");
        playerLeftOnPurpose = true;
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        LoadingController.LoadScene(LoadingController.Scenes.GPMP_LOBBY);
    }

    private void OnStartSearchForQuickMatch(object model) {
        GPMPMatchModel matchModel = (GPMPMatchModel)model;
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(matchModel.minimumAmountOpponents, matchModel.maximumAmountOpponents, 0, this);
        LoadingController.LoadScene(LoadingController.Scenes.GPMP_WAITING_ROOM);
    }

    private void OnStartInvitingForMatch(object model) {
        GPMPMatchModel matchModel = (GPMPMatchModel)model;
        PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(matchModel.minimumAmountOpponents, matchModel.maximumAmountOpponents, 0, this);
        LoadingController.LoadScene(LoadingController.Scenes.GPMP_WAITING_ROOM);
    }
    
    /*
     Functions handling on notification clicked for invitation
         */

    public void OnInvitationReceived(Invitation invitation, bool shouldAutoAccept) {
        if (shouldAutoAccept) {
            LoadingController.LoadScene(LoadingController.Scenes.GPMP_WAITING_ROOM);
            PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, this);
        }
    }


    /*
     RealTimeMultiplayerListener functions
         */

    public void OnRoomSetupProgress(float percentage) {
        DebugMP.Log("OnRoomSetupProgress");
        EventManager.TriggerEvent(GPMPEvents.Types.GPMP_REPORT_ROOM_SETUP_PROGRESS.ToString(), percentage);
    }

    public void OnRoomConnected(bool success) {
        DebugMP.Log("OnRoomConnected " + success);
        if (success) {
            EventManager.TriggerEvent(GPMPEvents.Types.GPMP_MATCH_MAKING_DONE.ToString());

            // Reset save model
            SaveLoadController.GetInstance().GetMultiplayerSession().Reset();

            // Start versusn screen
            LoadingController.LoadScene(LoadingController.Scenes.GPMP_VERSUS_SCREEN);
        } else {
            PlayGamesPlatform.Instance.RealTime.LeaveRoom();
            LoadingController.LoadScene(LoadingController.Scenes.GPMP_LOBBY);
        }
    }

    public void OnLeftRoom() {
        DebugMP.Log("OnLeftRoom");
    }

    public void OnParticipantLeft(Participant participant) {
        DebugMP.Log("OnParticipantLeft");
        playerLeftOnPurpose = false;
        errorMessage = "Your opponent left the game";
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        LoadingController.LoadScene(LoadingController.Scenes.GPMP_LOBBY);
    }

    public void OnPeersConnected(string[] participantIds) {
        DebugMP.Log("OnPeersConnected");
        EventManager.TriggerEvent(GPMPEvents.Types.GPMP_OPPONENT_FOUND.ToString());
    }

    public void OnPeersDisconnected(string[] participantIds) {
        DebugMP.Log("OnPeersDisconnected");
        playerLeftOnPurpose = false;
        errorMessage = "Your opponent left the game";
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        LoadingController.LoadScene(LoadingController.Scenes.GPMP_LOBBY);
    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data) {
        EventManager.TriggerEvent(GPMPEvents.Types.GPMP_MESSAGE_RECIEVED.ToString(), data);
    }
}
