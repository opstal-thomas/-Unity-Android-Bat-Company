using UnityEngine;
using System.Collections;

public class EventTypes {

    public const string GAME_START = "gameStart";
    public const string GAME_PAUSED = "gamePaused";
    public const string GAME_RESUME = "gameResume";
    public const string GAME_OVER = "gameOver";

    public const string NEW_HIGHSCORE = "newHighscore";

    public const string HEALTH_PICKED_UP = "healthPickedUp";
    public const string ECHO_USED = "echoUsed";
    public const string ECHO_USED_RESOURCES = "echoUsedResources";
    public const string SKILL_VALUE = "skillValue";
    public const string SHAPE_SHIFT = "shapeShift";
    public const string BLOOD_SENT = "bloodSent";
    public const string PLAYER_SPEED_CHANGED = "playerSpeedChanged";
    public const string PLAYER_DIED = "playerDied";
    public const string PLAYER_FLY_IN = "playerFlyIn";
    public const string PLAYER_TAKES_DAMAGE = "playerTakesDamage";
    public const string ENABLE_PLAYER_LIGHT = "enablePlayerLight";
    public const string DISABLE_PLAYER_LIGHT = "disablePlayerLight";
    public const string PLAYER_IN_POSITION = "playerInPosition";
    public const string CHANGE_BACKGROUND = "changeBackground";

    // SAVING LOADING
    public const string LOADING_SAVE_DATA = "loadingSaveData";
    public const string DONE_LOADING_SAVE_DATA = "doneLoadingSaveData";
    public const string START_SAVING_GAME = "startSavingGame";
    public const string DONE_SAVING_GAME = "doneSavingGame";

    //HIDE/SHOW & DISABLE/ENABLE Echo + UI Elements
    public const string HIDE_AND_DISABLE = "hideAndDisable";
    public const string SHOW_AND_ENABLE = "showAndEnable";

    public const string FADE_LIGHT_IN = "fadeLightIn";
    public const string FADE_LIGHT_OUT = "fadeLightOut";

    //COMBO SYSTEM
    public const string GOOD_ECHO = "goodEcho";
    public const string PERFECT_ECHO = "perfectEcho";
    public const string SPECIAL_USED = "specialUsed";
    public const string SWIPE_UP = "swipeUp";
    public const string CANCEL_SWIPE_UP = "cancelSwipeUp";

    //ENVIROMENTAL CHANGES
    public const string TRANSITION_START = "transitionStart";
    public const string TRANSITION_END = "transitionEnd";
    public const string CHANGE_ENVIRONMENT = "changeEnvironment";
    public const string WALL_SPRITES_UPDATED = "wallSpritesUpdated";
    public const string FLOOR_SPRITES_UPDATED = "floorSpritesUpdated";

    //NETWORK 
    public const string SERVER_STARTED = "serverStarted";
    public const string PLAYER_TWO_JOINED = "playerTwoJoined";
    public const string START_MATCH = "startMatch";
    public const string START_COUNTDOWN = "startCountdown";
    public const string INSTANTIATE_OBJECT_POOL = "instantiateObjectPool";
    public const string PLAY_ONLINE_PRESSED = "playOnlinePressed";
    public const string HIDE_LOBBY = "hideLobby";
    public const string RESTART_SEARCH = "restartSearch";

    //STORE
    public const string PURCHASE_STARTED = "purchaseStarted";
    public const string PURCHASE_CONFIRMED = "purchaseConfirmed";
    public const string PURCHASE_CANCELED = "purchaseCanceled";
    public const string NEW_SKIN_ACTIVE = "newSkinActive";

    //AUDIO
    public const string ENABLE_AUDIO = "enableAudio";
    public const string DISABLE_AUDIO = "disableAudio";
}
