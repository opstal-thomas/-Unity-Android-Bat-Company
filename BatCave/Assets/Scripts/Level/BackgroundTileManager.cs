using UnityEngine;
using System.Collections;

public class BackgroundTileManager : MonoBehaviour {
    public SpriteRenderer[] bottomTiles, topTiles;
    public GameObject transitionOut, transitionIn;

    public Sprite[] woodsSprites;
    public Sprite[] sprites;

    private float yBounds = -11f;
    public int treeTime;
    private int currentTreeTime;

    private bool isTransition = false;
    private bool isTransitionOut = false;
    private bool isTransitionIn = false;

    public float yOutOffSet = 0f;
    public float yInOffSet = -10f;
    private bool outOfIntro = true;


    // Use this for initialization
    void Start () {
        EventManager.StartListening(EventTypes.TRANSITION_START, StartTransition);
        EventManager.StartListening(EventTypes.TRANSITION_END, EndTransition);
    }


    private void StartTransition(object value) {
        //Change Sprites
        sprites = null; //reset just incase
        EnvironmentModel newSprites = value as EnvironmentModel;
        sprites = woodsSprites;
        isTransition = true;
        isTransitionOut = true;
    }

    private void EndTransition(object value) {
        sprites = null; //reset just incase
        EnvironmentModel newSprites = value as EnvironmentModel;
        sprites = newSprites.backgroundTiles as Sprite[];
        transitionOut.GetComponent<SpriteRenderer>().sprite = newSprites.transitionOut;
        transitionIn.GetComponent<SpriteRenderer>().sprite = newSprites.transitionIn;

        isTransitionIn = true;
    }

    void FixedUpdate () {
        CheckTilePosition();

        if (currentTreeTime >= (treeTime*4))
        {
            isTransition = false;
            currentTreeTime = 0;
            EventManager.TriggerEvent(EventTypes.CHANGE_ENVIRONMENT);

        }
    }

    private void SetRandomSprite(SpriteRenderer tile) {
        tile.sprite = sprites[Mathf.RoundToInt(Random.Range(0, sprites.Length))];
    }
    
    private void CheckTilePosition() {
        foreach (SpriteRenderer tile in bottomTiles) {
            if (tile.transform.position.y <= yBounds) {
                ResetTile(tile, topTiles[0].transform.position.y + topTiles[0].bounds.size.y);
            }
        }
        foreach (SpriteRenderer tile in topTiles) {
            if (tile.transform.position.y <= yBounds) {
                ResetTile(tile, bottomTiles[0].transform.position.y + bottomTiles[0].bounds.size.y);
            }
        }
    }
    /// <summary>
    /// Repositions the tile sprite back to the top and gives it a random sprite from the sprites array.
    /// </summary>
    /// <param name="tile"></param>
    private void ResetTile(SpriteRenderer tile, float yPos) {
        tile.transform.position = new Vector3(tile.transform.position.x, yPos, tile.transform.position.z);
        SetRandomSprite(tile);

        if (isTransition) {
            if (isTransitionOut) {
                transitionOut.transform.position = new Vector3(0, (yPos + yOutOffSet), -2);
                transitionOut.GetComponent<SimpleMoveScript>().enabled = true;
                isTransitionOut = false;
            }
            currentTreeTime++;
        }

        if (isTransitionIn)
        {
            transitionIn.transform.position = new Vector3(0, (yPos + yInOffSet), -2);
            transitionIn.GetComponent<SimpleMoveScript>().enabled = true;
            isTransitionIn = false;
        }
    }
}
