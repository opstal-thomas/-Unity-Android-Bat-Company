using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSpecialAbility : MonoBehaviour {
    public float speedDuration;
    public float shieldDuration;
    public float DayTimeUpTime;

    public Image specialCycler;
    public GameObject[] boosts;
    public int cycleAmount;
    public int flickerAmount;

    public Text swipeUpText;
    public Image backGround;
    private bool isEnabled;

    private Vector3 velocity = Vector3.zero;
    public AudioSource audioSource;

    private float cycleDelay;

    private void Start() {
        EventManager.StartListening(EventTypes.SPECIAL_USED, SpecialUsed);
        EventManager.StartListening(EventTypes.SWIPE_UP, SwipeUpAnimation);
        EventManager.StartListening(EventTypes.CANCEL_SWIPE_UP, CancelSwipeUp);
    }

    private void OnDestroy() {
        EventManager.StopListening(EventTypes.SPECIAL_USED, SpecialUsed);
        EventManager.StopListening(EventTypes.SWIPE_UP, SwipeUpAnimation);
        EventManager.StopListening(EventTypes.CANCEL_SWIPE_UP, CancelSwipeUp);
    }

    private void CancelSwipeUp(object arg0) {
        CancelInvoke("StartAnimation");
        swipeUpText.enabled = false;
        isEnabled = false;
    }

    private void SwipeUpAnimation(object arg0) {
        InvokeRepeating("StartAnimation", 0, 0.5f);
    }

    private void StartAnimation() {
        if (isEnabled)
        {
            swipeUpText.enabled = false;
            isEnabled = false;
        }
        else {
            swipeUpText.enabled = true;
            isEnabled = true;
        }
    }

    private void SpecialUsed(object value) {
        CancelInvoke("StartAnimation");
        swipeUpText.enabled = false;
        isEnabled = false;

        backGround.transform.localScale = new Vector3(0,0,0);
        backGround.GetComponent<Image>().enabled = true;

        specialCycler.enabled = true;
        cycleDelay = 0.05f;
        StartCoroutine(CycleSpritesDelay());
    }

    IEnumerator CycleSpritesDelay()
    {
        backGround.transform.localScale = Vector3.MoveTowards(backGround.transform.localScale, new Vector3(1,1,1), 0.25f);
        yield return new WaitForSeconds(0.025f);
        backGround.transform.localScale = Vector3.MoveTowards(backGround.transform.localScale, new Vector3(1, 1, 1), 0.25f);
        yield return new WaitForSeconds(0.025f);
        backGround.transform.localScale = Vector3.MoveTowards(backGround.transform.localScale, new Vector3(1, 1, 1), 0.25f);
        yield return new WaitForSeconds(0.025f);
        backGround.transform.localScale = Vector3.MoveTowards(backGround.transform.localScale, new Vector3(1, 1, 1), 0.25f);
        yield return new WaitForSeconds(0.025f);
        backGround.transform.localScale = Vector3.MoveTowards(backGround.transform.localScale, new Vector3(1, 1, 1), 0.25f);
        yield return new WaitForSeconds(0.025f);
        backGround.transform.localScale = Vector3.MoveTowards(backGround.transform.localScale, new Vector3(1, 1, 1), 0.25f);

        for (int i = 0; i < cycleAmount; i++)
        {
            for (int j = 0; j < boosts.Length; j++)
            {
                yield return new WaitForSeconds(cycleDelay);
                specialCycler.sprite = boosts[j].GetComponent<SpriteRenderer>().sprite;
                audioSource.Play();
            }
            cycleDelay += 0.05f;
        }

        GameObject boostToActivate = boosts[Random.Range(0, boosts.Length)];
        specialCycler.sprite = boostToActivate.GetComponent<SpriteRenderer>().sprite;

        for (int i = 0; i < flickerAmount; i++) {
            specialCycler.enabled = false;
            yield return new WaitForSeconds(0.1f);
            specialCycler.enabled = true;
            audioSource.Play();
            yield return new WaitForSeconds(0.1f);
        }

        ActivateBooster(boostToActivate);

        yield return new WaitForSeconds(2);
        specialCycler.enabled = false;
        backGround.GetComponent<Image>().enabled = false;
        StopCoroutine(CycleSpritesDelay());
    }

    private void ActivateBooster(GameObject booster) {
        string boosterName = booster.name;

        switch (boosterName) {
            case "SpeedBooster":
                EventManager.TriggerEvent(PowerupEvents.PLAYER_SPEED_PICKUP, speedDuration);
                Debug.Log("Speed boost active");
                break;
            case "ShieldBooster":
                EventManager.TriggerEvent(PowerupEvents.PLAYER_SHIELD_PICKUP, shieldDuration);
                Debug.Log("Shield boost active");
                break;
            case "DayTimeBooster":
                EventManager.TriggerEvent(PowerupEvents.PLAYER_LIGHT_PICKUP, DayTimeUpTime);
                Debug.Log("Light boost active");
                break;
            default:
                Debug.Log("404 Booster not found!");
                break;
        }
    }
}
