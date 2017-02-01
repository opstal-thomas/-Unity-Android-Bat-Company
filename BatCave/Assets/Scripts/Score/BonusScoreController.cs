using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BonusScoreController : MonoBehaviour {

    public EndScreenScore endScoreScreen;
    public Animator scorePanelAnimator;
    private Animator animator;
    public Text amountText;
    public Image image;

    private ArrayList bonusQueue;
    private bool isPlaying;
    private bool itemIsPlaing;

    private float animationCheckDelay = 2f;
    private float animationCheckTimer;

	void Start () {
        animator = GetComponent<Animator>();
        bonusQueue = new ArrayList();
        isPlaying = false;
    }
	
	void Update () {

        // if the PlayQueue function has been called from outside
        if (isPlaying) {

            // if current animation is still playing 
            if (animationCheckTimer <= 0f && this.animator.GetCurrentAnimatorStateInfo(0).IsName("FinishedState")) {
                if (QueueHasNext()) {
                    ShowNextBonus();
                    animator.SetTrigger("Play");
                    animationCheckTimer = animationCheckDelay;
                } else {
                    isPlaying = false;
                    scorePanelAnimator.SetTrigger("Play");
                }
            } else {
                animationCheckTimer -= Time.deltaTime;
            }
        }
    }

    bool QueueHasNext() {
        return bonusQueue.Count > 0;
    }

    void ShowNextBonus() {
        BonusPointModel bpm = (BonusPointModel)bonusQueue[0];
        image.sprite = bpm.GetVisual();
        amountText.text = "x" + bpm.GetAmount().ToString();
        endScoreScreen.AddScoreToQueue(bpm.GetScorePerAmount() * bpm.GetAmount(), 1.8f);
        bonusQueue.Remove(bpm);
    }

    public void AddBonusToQueue(int amount, float scorePerAmount, Sprite visual) {
        BonusPointModel bpm = new BonusPointModel(amount, scorePerAmount, visual);
        bonusQueue.Add(bpm);
    }

    public void PlayQueue() {
        isPlaying = true;
    }

    class BonusPointModel {
        private int amount;
        private float scorePerAmount;
        private Sprite visual;

        public BonusPointModel(int amount, float scorePerAmount, Sprite visual) {
            this.amount = amount;
            this.scorePerAmount = scorePerAmount;
            this.visual = visual;
        }

        public float GetScorePerAmount() {
            return this.scorePerAmount;
        }

        public int GetAmount() {
            return this.amount;
        }

        public Sprite GetVisual() {
            return this.visual;
        }
    }
}
