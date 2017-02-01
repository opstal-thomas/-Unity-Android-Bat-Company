using UnityEngine;
using System.Collections;
using GooglePlayGames;
using System;
using System.Collections.Generic;

public class GPMPPlayerView : MonoBehaviour {

    public float networkCallDelay;
    public float speed;
    public Vector3 playerOneSpawnpoint;
    public Vector3 playerTwoSpawnpoint;
    public GameObject[] playerEchos;
    public SkillSlider skillSlider;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private bool playerLeft;
    private bool playerRight;
    private float xPosition;
    private Vector2 fp;
    private Vector2 lp;
    private bool touchStarted;
    private float lastNetworkCall;
    private GPMPMatchModel matchModel;
    private bool paused = true;
    private int activeSkinID;

    
    void Start () {
        // Player me should listen to the player and update its position to the other player
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        EventManager.StartListening(GPMPEvents.Types.GPMP_MATCH_INFO_READY.ToString(), OnMatchInfoReady);
        EventManager.StartListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnMatchStarted);
        EventManager.StartListening(EventTypes.SKILL_VALUE, OnSkillValueRecieved);

        // Set player skin 
        activeSkinID = SaveLoadController.GetInstance().GetPlayer().GetActiveSkinID();
        animator.SetTrigger(activeSkinID.ToString());
    }

    void OnDestroy() {
        EventManager.StopListening(GPMPEvents.Types.GPMP_MATCH_INFO_READY.ToString(), OnMatchInfoReady);
        EventManager.StopListening(GPMPEvents.Types.GPMP_START_GAME.ToString(), OnMatchStarted);
        EventManager.StopListening(EventTypes.SKILL_VALUE, OnSkillValueRecieved);

    }

    private void OnMatchInfoReady(object model) {
        matchModel = (GPMPMatchModel)model;
        if (matchModel.iAmTheHost) {
            rigidbody.position = playerOneSpawnpoint;
        } else {
            rigidbody.position = playerTwoSpawnpoint;
        }
        xPosition = rigidbody.position.x;
    }
    
    void Update() {
        if (paused)
            return;

        CheckPlayerPosition();
        //Swipe Controls
        Vector3 pos = rigidbody.position;
        pos.x = Mathf.MoveTowards(pos.x, xPosition, speed * Time.deltaTime);
        rigidbody.position = pos;
        CheckForSwipe();
        rigidbody.AddForce(transform.forward * speed * Time.deltaTime, ForceMode2D.Force);

        if (lastNetworkCall >= networkCallDelay) {
            lastNetworkCall = 0;
            EventManager.TriggerEvent(GPMPEvents.Types.GPMP_UPDATE_MY_POSITION.ToString(), transform.position);
        } else {
            lastNetworkCall += Time.deltaTime;
        }
    }

    private void OnMatchStarted(object arg0) {
        paused = false;
    }

    void CheckPlayerPosition() {
        playerLeft = false;
        playerRight = false;

        if (xPosition > 1.8) {
            playerRight = true;
        } else if (xPosition < -1.8) {
            playerLeft = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Obstacle") {
            GameItem item = col.gameObject.GetComponent<GameItem>();
            for (int i = 0; i < item.laneWeight; i++) {
                //DebugMP.Log("My pos x: " + transform.position.x + "\t" + "obstacle pos x: " + col.transform.position.x + (i * 1));
                if (transform.position.x < col.transform.position.x + (i * 1) + 0.5 && transform.position.x > col.transform.position.x + (i * 1) - 0.5) {
                    GetComponent<ParticleSystem>().Play();
                    GetComponent<SpriteRenderer>().enabled = false;
                    EventManager.TriggerEvent(GPMPEvents.Types.GPMP_PLAYER_DIED.ToString());
                    break;
                }
            }
        }
    }

    public void SpawnEcho() {
        EventManager.TriggerEvent(EventTypes.ECHO_USED);
    }

    void OnSkillValueRecieved(object arg0) {
        foreach (GameObject echo in playerEchos) {
            if (!echo.activeInHierarchy) {
                echo.SetActive(true);
                echo.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
                echo.GetComponent<MoveEcho>().EchoSize(skillSlider.GetLastSkillValue());
                return;
            }
        }
    }

    void CheckForSwipe() {
        // DEBUG CODE (Editor debug)
        if (Application.isEditor) {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) {
                SpawnEcho();
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
                playerLeft = true;
                xPosition -= 1;
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) {
                playerRight = true;
                xPosition += 1;
            }
            return;
        }

        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                fp = touch.position;
                lp = touch.position;
            }
            if (touch.phase == TouchPhase.Moved) {
                lp = touch.position;
            }
            if (touch.phase == TouchPhase.Moved) {
                if ((fp.x - lp.x) > 10 && !playerLeft && !touchStarted) // left swipe
                {
                    touchStarted = true;
                    xPosition -= 1;
                } else if ((fp.x - lp.x) < -10 && !playerRight && !touchStarted) // right swipe
                  {
                    touchStarted = true;
                    xPosition += 1;
                }
            }

            if (touch.phase == TouchPhase.Ended) {
                if ((fp.x - lp.x) > 10) {
                    touchStarted = false;
                } else if ((fp.x - lp.x) < -10) {
                    touchStarted = false;
                } else if ((fp.x - lp.x) > -3 && fp.y < (Screen.height - Screen.height / 4)) {
                    SpawnEcho();
                } else if ((fp.x - lp.x) < 3 && fp.y < (Screen.height - Screen.height / 4)) {
                    SpawnEcho();
                }
            }
        }
    }

}
