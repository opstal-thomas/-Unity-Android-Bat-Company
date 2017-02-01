using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class EndScreenScore : MonoBehaviour {

    public Text totalGamesPlayedTextField;
    public Text lastScoreTextField;
    public BonusScoreController bonusScoreController;
    private float scoreAddPointDelay;

    private float scoreAddPointDelayCounter;
    public Sprite goodBonusSprite;
    public Sprite excellentBonusSprite;
    private float totalScoreInQueue;

    private float shownScore;
    private float extraShowDelay;

    private bool addShown = false;

	// Use this for initialization
	void Start () {
        EndlessSessionSave gameSession = SaveLoadController.GetInstance().GetEndlessSession();
        //totalGamesPlayedTextField.text = "Games: " + player.GetTotalGamesPlayed();

        totalScoreInQueue = gameSession.GetScore();
        
        scoreAddPointDelay = 0.01f / totalScoreInQueue;

        if (gameSession.GetEchosTimedGood() > 0)
            bonusScoreController.AddBonusToQueue((int)gameSession.GetEchosTimedGood(), gameSession.GetGoodEchoPoint(), goodBonusSprite);

        if (gameSession.GetEchosTimedExcellent() > 0)
            bonusScoreController.AddBonusToQueue((int)gameSession.GetEchosTimedExcellent(), gameSession.GetExcellentEchoPoint(), excellentBonusSprite);

        bonusScoreController.PlayQueue();

    }

    void FixedUpdate() {
        if (extraShowDelay <= 0) {
            if (totalScoreInQueue > 0 && totalScoreInQueue % 200 == 0) {
                if (scoreAddPointDelayCounter >= scoreAddPointDelay) {
                    shownScore += 200;
                    totalScoreInQueue -= 200;
                    lastScoreTextField.text = shownScore.ToString();
                    scoreAddPointDelayCounter = 0;
                } else {
                    scoreAddPointDelayCounter += Time.deltaTime;
                }
            } else if (totalScoreInQueue > 0) {
                shownScore += totalScoreInQueue;
                lastScoreTextField.text = shownScore.ToString();
                totalScoreInQueue = 0;
            }
        } else {
            extraShowDelay -= Time.deltaTime;
        }
    }

    public void AddScoreToQueue(float score, float extraDelay) {
        totalScoreInQueue += score;
        this.extraShowDelay = extraDelay;
    }

    void Update() {

        if (!addShown) {
            if (Random.Range(1, 101) <= 20)
            {
                ShowAd();
            }
            else {
                addShown = true;
            }
        }
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady() && !addShown)
        {
            Advertisement.Show();
            addShown = true;
        }
    }
}
