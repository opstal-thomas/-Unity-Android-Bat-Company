using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
    public GameObject Echo;
    public Transform PlayerPos;
    public float playerYposition;
    public PlayerResources playerResources;
    public ScoreCalculator score;
    public SkillSlider skillSlider;
    public GameObject[] playerEchos;
    public Light playerLight;
    public float lightFadeSpeed;
    public float playerScaleUpSpeed;
    public bool echoEnabled;
    public AudioClip damageSound;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource audioSource;

    //movement
    private Vector2 movement;
    public float speed = 30;
    private Vector2 fp; // first finger position
    private Vector2 lp; // last finger position
    private float xPosition;
    private bool playerLeft;
    private bool playerRight;

    private bool touchStarted = false;
    private bool isPaused;
    private bool playerIsDead;
    private bool controlsEnabled;
    private bool lightIsFadingIn;
    private bool lightIsFadingOut;

    private bool canDie = true;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //speed = SaveLoadController.GetInstance().GetOptions().GetControlSensitivity();
        xPosition = rigidbody.position.x;
        EventManager.StartListening(EventTypes.SKILL_VALUE, OnSkillValueRecieved);
        EventManager.StartListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StartListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StartListening(EventTypes.PLAYER_DIED, OnPlayerDied);
        EventManager.StartListening(EventTypes.ENABLE_PLAYER_LIGHT, OnPlayerLightEnabled);
        EventManager.StartListening(EventTypes.DISABLE_PLAYER_LIGHT, OnPlayerLightDisabled);
        EventManager.StartListening(EventTypes.PLAYER_IN_POSITION, OnPlayerInPosition);

        animator.SetTrigger(SaveLoadController.GetInstance().GetPlayer().GetActiveSkinID().ToString());
    }

    void OnDestroy() {
        EventManager.StopListening(EventTypes.SKILL_VALUE, OnSkillValueRecieved);
        EventManager.StopListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StopListening(EventTypes.GAME_PAUSED, OnGamePaused);
        EventManager.StopListening(EventTypes.PLAYER_DIED, OnPlayerDied);
        EventManager.StopListening(EventTypes.ENABLE_PLAYER_LIGHT, OnPlayerLightEnabled);
        EventManager.StopListening(EventTypes.DISABLE_PLAYER_LIGHT, OnPlayerLightDisabled);
        EventManager.StopListening(EventTypes.PLAYER_IN_POSITION, OnPlayerInPosition);
    }
    
    void OnGamePaused(object arg0) {
        isPaused = true;
    }

    void OnGameResume(object arg0) {
        StartCoroutine(WaitAbit());
    }

    void OnPlayerLightEnabled(object arg0) {
        lightIsFadingIn = true;
        lightIsFadingOut = false;
    }

    void OnPlayerLightDisabled(object arg0) {
        lightIsFadingIn = false;
        lightIsFadingOut = true;
    }

    IEnumerator WaitAbit() {
        yield return new WaitForSeconds(1);
        isPaused = false;
    }



    void OnPlayerDied(object arg0) {
        // Hide player sprite
        GetComponent<SpriteRenderer>().enabled = false;

        // Start blood effect
        GetComponent<ParticleSystem>().Play();

        playerIsDead = true;
    }

    void OnPlayerInPosition(object arg0) {
        transform.position = new Vector3(transform.position.x, playerYposition, transform.position.z);
        controlsEnabled = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    void Update() {
        if (!isPaused && !playerIsDead && controlsEnabled)
        {
            CheckPlayerPosition();
            //Swipe Controls
            Vector2 pos = rigidbody.position;
            pos.x = Mathf.MoveTowards(pos.x, xPosition, speed * Time.deltaTime);
            rigidbody.position = pos;
            CheckForSwipe();
            rigidbody.AddForce(transform.forward * speed * Time.deltaTime, ForceMode2D.Force);
        }
    }

    void FixedUpdate()
    {
        if (!isPaused && !playerIsDead) {
            if (lightIsFadingIn) {
                playerLight.intensity = Mathf.Lerp(playerLight.intensity, 8f, lightFadeSpeed * Time.deltaTime);
                if (playerLight.intensity >= 7f) {
                    lightIsFadingIn = false;
                    playerLight.intensity = 8f;
                }
            }

            if (lightIsFadingOut) {
                playerLight.intensity = Mathf.Lerp(playerLight.intensity, 0f, lightFadeSpeed * Time.deltaTime);
                if (playerLight.intensity >= 1f) {
                    lightIsFadingOut = false;
                    playerLight.intensity = 0f;
                }
            }

        }
    }

    public void SpawnEcho() {
        if (echoEnabled) {
            EventManager.TriggerEvent(EventTypes.ECHO_USED);
        }
    }

    private void UseSpecial() {
        if (playerResources.echoComboAmount == playerResources.maxEchoComboAmount) {
            EventManager.TriggerEvent(EventTypes.SPECIAL_USED);
        }
    }

    void OnSkillValueRecieved(object arg0) {
        foreach (GameObject echo in playerEchos)
        {
            if (!echo.activeInHierarchy)
            {
                echo.SetActive(true);
                echo.transform.position = new Vector3(PlayerPos.position.x, PlayerPos.position.y, -2);
                echo.GetComponent<MoveEcho>().EchoSize(skillSlider.GetLastSkillValue());
                return;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            // take damage
            if (canDie) {
                playerResources.removeHealth(playerResources.damageAmount);
                EventManager.TriggerEvent(EventTypes.PLAYER_TAKES_DAMAGE);
                GetComponent<ParticleSystem>().Play();
                audioSource.clip = damageSound;
                audioSource.Play();
            } 

            // check if player died
            if (playerResources.health <= 0)
                EventManager.TriggerEvent(EventTypes.PLAYER_DIED);
        }
    }

    void CheckPlayerPosition() {
        playerLeft = false;
        playerRight = false;

        if (xPosition > 1.8)
        {
            playerRight = true;
        }

        else if (xPosition < -1.8)
        {
            playerLeft = true;
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

            if (Input.GetKeyUp(KeyCode.P)) {
                UseSpecial();
            }

            if (Input.GetKeyUp(KeyCode.O)) {
                animator.SetBool("isBoosting", false);
            }
            return;
        }
        

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;

                if ((fp.y - lp.y) < -150)
                {
                    touchStarted = true;
                    UseSpecial();
                }
                else if ((fp.x - lp.x) > 20 && !playerLeft && !touchStarted) // left swipe
                {
                    touchStarted = true;
                    xPosition -= 1;
                }
                else if ((fp.x - lp.x) < -20 && !playerRight && !touchStarted) // right swipe
                {
                    touchStarted = true;
                    xPosition += 1;
                }
            }

            if (touch.phase == TouchPhase.Ended) {
                if ((fp.x - lp.x) > 20){
                    touchStarted = false;
                }
                else if ((fp.x - lp.x) < -20){
                    touchStarted = false;
                }
                else if ((fp.x - lp.x) > -3 && fp.y < (Screen.height - Screen.height/4)) {
                    SpawnEcho();
                } else if ((fp.x - lp.x) < 3 && fp.y < (Screen.height - Screen.height / 4)) {
                    SpawnEcho();
                }
            }
        }
    }

    public void SetShield(bool shieldActive) {
        if (shieldActive) {
            canDie = false;
        }

        if (!shieldActive) {
            canDie = true;
        }
    }
}
