using UnityEngine;
using System.Collections;
using System;

public class MoveEcho : MonoBehaviour {
    public float maxUpTime;
    public float upTime;
    public Vector2 Speed = new Vector2(0.1f, 0.1f);
    private Rigidbody2D rb;
    public Light echo;
    public AudioClip[] sounds;
    private AudioSource audioSource;

    private float maxSpotAngle;
    public float defaultSpotAngle = 120;
    private bool isPaused;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        EventManager.StartListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StartListening(EventTypes.GAME_PAUSED, OnGamePaused);
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(Speed.x, Speed.y);
        }

        echo.intensity = 0;
        echo.spotAngle = 50;
    }

    void PlayRandomSound() {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.clip = sounds[UnityEngine.Random.Range(0, sounds.Length - 1)];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (rb == null)
            {
                transform.Translate(Speed.x, Speed.y, 0);
            }

            upTime -= Time.deltaTime;

            if (upTime < 0)
            {
                gameObject.SetActive(false);
            }

            if (echo.spotAngle < maxSpotAngle)
            {
                echo.spotAngle += 3;
            }

            if (echo.intensity < 8)
            {
                echo.intensity += 1;
            }
        }
    }

    void OnDisable() {
        upTime = maxUpTime;
        echo.intensity = 0;
        echo.spotAngle = 50;
    }

    public void EchoSize(float value) {
        maxSpotAngle = -((Mathf.Pow(value, 2) / 25)) + (4 * value) + 20;
        PlayRandomSound();
    }

    void OnDestroy()
    {
        EventManager.StopListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StopListening(EventTypes.GAME_PAUSED, OnGamePaused);
    }

    private void OnGamePaused(object arg0)
    {
        isPaused = true;
    }

    private void OnGameResume(object arg0)
    {
        isPaused = false;
    }
}
