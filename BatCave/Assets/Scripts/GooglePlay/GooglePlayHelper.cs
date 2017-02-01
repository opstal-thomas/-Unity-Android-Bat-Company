using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using UnityEngine;
using GooglePlayGames.BasicApi.Multiplayer;

/// <summary>
/// This helper class functions as a layer between the Google Play 
/// plugin code and the game code.
/// </summary>
public class GooglePlayHelper {

    private const string DEBUG_KEY = "GOOGLE_PLAY_HELPER    ";

    public static GooglePlayHelper instance;
    public static string currentSaveFileName;
    public const string CURRENT_SAVE_FILE_KEY = "CURRENT_SAVE_FILE_KEY";

    private bool isInitialized;
    private PlayGamesClientConfiguration config;
    private ISavedGameMetadata saveGameMetaData;
    public bool isSaving;
    public bool isLoading;

    public static GooglePlayHelper GetInstance() {
        if (instance == null)
            instance = new GooglePlayHelper();
        return instance;
    }

    public GooglePlayHelper() {
        if (!isInitialized)
            Init();
    }

    /// <summary>
    /// Returns true if the player is authenticated with Google Play
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerAuthenticated() {
        return Social.localUser.authenticated;
    }

    /// <summary>
    /// Initializes Google Play Service
    /// </summary>
    public void Init() {
        // Enables saving game progress.
        config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .WithInvitationDelegate(GPMPController.GetInstance().OnInvitationReceived)
            .Build();
        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        // Set Google Play Service as Social platform
        PlayGamesPlatform.Activate();

        isInitialized = true;
    }
    
    /// <summary>
    /// Starts Google Authentication flow for authenticating player
    /// </summary>
    public void Login() {
        if (!Social.localUser.authenticated) {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success) {
                    Debug.Log(DEBUG_KEY + "Login succes");
                    PlayerPrefs.SetInt("PlayerRefusedGooglePLay", 0);
                    CheckForSaveGame();
                } else {
                    Debug.Log(DEBUG_KEY + "Login failed!");
                    PlayerPrefs.SetInt("PlayerRefusedGooglePLay", 1);
                }
            });
        } else {
            Debug.Log(DEBUG_KEY + "Player already authenticated.");
            PlayerPrefs.SetInt("PlayerRefusedGooglePLay", 0);
            CheckForSaveGame();
        }
    }

    private void CheckForSaveGame() {
        isLoading = true;
        currentSaveFileName = PlayerPrefs.GetString(CURRENT_SAVE_FILE_KEY);
        Debug.Log(DEBUG_KEY + "Current save file: " + currentSaveFileName);
        if (currentSaveFileName == string.Empty) {
            SelectSaveGame();
        } else {
            OpenSavedGame(currentSaveFileName);
        }
    }

    public void SelectSaveGame() {
        uint maxNumToDisplay = 3;
        bool allowCreateNew = true;
        bool allowDelete = true;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ShowSelectSavedGameUI("Select save game",
            maxNumToDisplay,
            allowCreateNew,
            allowDelete,
            (SelectUIStatus status, ISavedGameMetadata saveGame) => {
                // some error occured, just show window again
                if (status == SelectUIStatus.SavedGameSelected) {
                    OpenSavedGame(saveGame);
                } 
            });
    }

    /// <summary>
    /// Logs out current active user from Google Play
    /// </summary>
    public void Logout() {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    /// <summary>
    /// Opens the saved game.
    /// </summary>
    /// <param name="savedGame">Saved game.</param>
    /// <param name="callback">Invoked when game has been opened</param>
    private void OpenSavedGame(ISavedGameMetadata savedGame) {
        if (savedGame == null) {
            Debug.Log(DEBUG_KEY + "Savegame is null");
            return;
        }
        if (!savedGame.IsOpen) {
            Debug.Log(DEBUG_KEY + "Opening save game meta data...");
            ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;

            string fileName;
            if (savedGame.Filename == string.Empty) {
                fileName = "Save" + UnityEngine.Random.Range(1000000, 9999999).ToString();
            } else {
                fileName = savedGame.Filename;
            }

            PlayerPrefs.SetString(CURRENT_SAVE_FILE_KEY, fileName);

            // save name is generated only when save has not been commited yet
            saveGameClient.OpenWithAutomaticConflictResolution(
                fileName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                OnSavedGameOpened);
        } else {
            Debug.Log(DEBUG_KEY + "Save game meta data was already open.");
            OnSavedGameOpened(SavedGameRequestStatus.Success, savedGame);
        }
    }

    private void OpenSavedGame(string filename) {
        if (filename == string.Empty || filename == null) {
            Debug.Log(DEBUG_KEY + "Filename passed is empty");
        }
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }


    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            Debug.Log(DEBUG_KEY + "Old meta data: " + saveGameMetaData);
            Debug.Log(DEBUG_KEY + "New meta data: " + game);
            saveGameMetaData = game;

            if (isSaving) {
                isSaving = false;
                Debug.Log(DEBUG_KEY + "Saving game...");
                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
                builder = builder
                    .WithUpdatedDescription("Saved game at " + DateTime.Now);
                SavedGameMetadataUpdate updatedMetadata = builder.Build();
                byte[] savedData = SaveLoadController.GetInstance().CreateSaveObject();
                savedGameClient.CommitUpdate(saveGameMetaData, updatedMetadata, savedData, OnSavedGameOpened);
                EventManager.TriggerEvent(EventTypes.DONE_SAVING_GAME);
            } else if (isLoading){
                isLoading = false;
                LoadGameData(saveGameMetaData);
            }
        } else {
            CheckForSaveGame();
            /*Debug.LogError(DEBUG_KEY + "Error opening/updating save game. Request status: " + status);
            Debug.LogError(DEBUG_KEY + "Metadata: " + game);
            Debug.LogError(DEBUG_KEY + "filename: " + game.Filename);
            Debug.LogError(DEBUG_KEY + "isOpen: " + game.IsOpen);
            Debug.LogError(DEBUG_KEY + "totaltimeplayed: " + game.TotalTimePlayed);
            Debug.LogError(DEBUG_KEY + "lastmodifiedstamp: " + game.LastModifiedTimestamp);*/
        }
    }

    /// <summary>
    /// Send savedata to Google Play service as an array of bytes
    /// </summary>
    /// <param name="game"></param>
    /// <param name="savedData"></param>
    /// <param name="totalPlaytime"></param>
    public void SaveGame() {
        if (Application.isEditor || !Social.localUser.authenticated)
            return;
        isSaving = true;

        // first op save game before writing
        OpenSavedGame(currentSaveFileName);
    }

    /// <summary>
    /// Load savegame data from Google Play Service
    /// </summary>
    /// <param name="game"></param>
    public void LoadGameData(ISavedGameMetadata game) {
        isLoading = false;
        Debug.Log(DEBUG_KEY + "Read save binary");
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }

    /// <summary>
    /// Callback from function LoadGameData() holding the result
    /// </summary>
    /// <param name="status"></param>
    /// <param name="data"></param>
    public void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data) {
        if (status == SavedGameRequestStatus.Success) {
            // handle processing the byte array data
            if (data.Length == 0) {
                // no save file is present
                Debug.Log(DEBUG_KEY + "No save game found (empty)");
            } else {
                // restore data
                SaveLoadController.GetInstance().RestoreSave(data);
            }
            EventManager.TriggerEvent(EventTypes.DONE_LOADING_SAVE_DATA);
        } else {
            // handle error
        }
    }

    public void PostHighscore(float score, string board) {
        long scoreLong = Convert.ToInt64(score);
        if (Application.isEditor)
            return;
        Social.ReportScore(scoreLong, board, (bool success) => {
            Debug.Log(DEBUG_KEY + "Highscore post status : " + success);
        });
    }

    public void UnlockAchievement(string achievementID) {
        if (Application.isEditor)
            return;
        Social.ReportProgress(achievementID, 100.0f, (bool success) => {
            Debug.Log(DEBUG_KEY + "Achievement unlocked status : " + success);
        });
    }
    
    public void ReportEvent(string eventName, float value) {
        uint u = Convert.ToUInt32(value);
        ReportEvent(eventName, u);
    }

    public void ReportEvent(string eventName, uint value) {
        if (Application.isEditor) {
            return;
        }

        Debug.Log(DEBUG_KEY + "Reporting event : " + eventName);
        PlayGamesPlatform.Instance.Events.IncrementEvent(eventName, value);
    }

    public void ShowAchievementsUI() {
        Social.ShowAchievementsUI();
    }

    public void ShowLeaderboardUI() {
        Social.ShowLeaderboardUI();
    }
}
