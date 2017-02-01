using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillSliderFeedback : MonoBehaviour {

    private Animator animator;
    private Text textComponent;
    public Color32 goodColor;
    public Color32 excellentColor;
    private AudioSource audioSource;

    public enum Types {
        GOOD,
        EXCELLENT
    }
    
    void Start () {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        textComponent = GetComponent<Text>();
    }

    public void TriggerFeedback(Types type) {
        ShowFeedback(type);
    }

    void ShowFeedback(Types type) {
        switch (type) {
            case Types.GOOD:
                textComponent.text = "Good!";
                textComponent.color = goodColor;
                animator.Play("FeedbackTextAnimation");
                EventManager.TriggerEvent(EventTypes.GOOD_ECHO);
                break;
            case Types.EXCELLENT:
                textComponent.text = "Excellent!";
                textComponent.color = excellentColor;
                animator.Play("FeedbackTextAnimation");
                EventManager.TriggerEvent(EventTypes.PERFECT_ECHO);
                audioSource.Play();
                break;
        }
    }
}
