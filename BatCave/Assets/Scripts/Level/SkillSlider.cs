using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillSlider : MonoBehaviour {

    public float speed;
    public float coolDown;
    public SkillSliderFeedback skillFeedbackController;

    private Slider slider;
    private bool isMovingLeft;
    private float coolDownTimer;
    private bool onCooldown;

    private float lastSkillValue;
    private bool isPaused;

    // fields for keeping track of the amount of timed echos in a row
    private int excellentEchosSequence;
    private int goodEchosSequence;

    // Use this for initialization
    void Start () {
        slider = GetComponent<Slider>();
        StartAtRandomPosition();
        coolDownTimer = 0f;
        
        EventManager.StartListening(EventTypes.ECHO_USED, OnSkillShotTriggered);
        EventManager.StartListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StartListening(EventTypes.GAME_PAUSED, OnGamePaused);
    }

    void OnGamePaused(object arg0) {
        isPaused = true;
    }

    void OnGameResume(object arg0) {
        isPaused = false;
        StartAtRandomPosition();
    }

    void OnDestroy() {
        EventManager.StopListening(EventTypes.ECHO_USED, OnSkillShotTriggered);
        EventManager.StopListening(EventTypes.GAME_RESUME, OnGameResume);
        EventManager.StopListening(EventTypes.GAME_PAUSED, OnGamePaused);
    }

    void Update() {
        if (!isPaused) {
            if (onCooldown)
                UpdateCoolDown();
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!isPaused) {
            if (!onCooldown)
                Move();
        }
	}

    void StartAtRandomPosition() {
        slider.value = Random.Range(slider.minValue, slider.maxValue);
        isMovingLeft = Random.value > 0.5f;
    }

    void UpdateCoolDown() {
        coolDownTimer -= Time.deltaTime;
        if (coolDownTimer <= 0f) {
            onCooldown = false;
            coolDownTimer = coolDown;
            StartAtRandomPosition();
        }
    }

    void Move() {
        float direction = isMovingLeft ? -1 : 1;
        if (isMovingLeft) {
            isMovingLeft = slider.value > slider.minValue;
        } else {
            isMovingLeft = slider.value >= slider.maxValue;
        }
        slider.value += (direction * speed * Time.deltaTime);
    }

    void OnSkillShotTriggered(object arg0) {
        if (onCooldown) 
            return;

        // set value
        lastSkillValue = slider.value;

        // set feedback text
        TriggerFeedbackText(lastSkillValue);

        // check for sequence achievements
        AchievementChecker.CheckForTimingAchievement(excellentEchosSequence, goodEchosSequence);

        // dispatch value for the beam
        EventManager.TriggerEvent(EventTypes.SKILL_VALUE);
        EventManager.TriggerEvent(EventTypes.ECHO_USED_RESOURCES);

        // Activate cooldown
        ResetCoolDown();
    }

    void ResetCoolDown() {
        onCooldown = true;
        coolDownTimer = coolDown;
    }

    void TriggerFeedbackText(float value) {
        if (value >= 49 && value <= 51) {
            skillFeedbackController.TriggerFeedback(SkillSliderFeedback.Types.EXCELLENT);
            SaveLoadController.GetInstance().GetEndlessSession().AddEchosTimedExcellent(1);
            excellentEchosSequence++;
            goodEchosSequence++;
            return;
        }
        
        if (value >= 40 && value <= 60) {
            skillFeedbackController.TriggerFeedback(SkillSliderFeedback.Types.GOOD);
            SaveLoadController.GetInstance().GetEndlessSession().AddEchosTimedGood(1);
            goodEchosSequence++;
            excellentEchosSequence = 0;
            return;
        }

        excellentEchosSequence = 0;
        goodEchosSequence = 0;
    }

    public float GetLastSkillValue() {
        return lastSkillValue;
    }
}
