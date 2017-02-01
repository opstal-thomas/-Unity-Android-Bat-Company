using UnityEngine;
using UnityEngine.UI;

public class WallManager : MonoBehaviour {
    public SpriteRenderer[] bottomWalls;
    public SpriteRenderer[] topWalls;
    public Sprite[] woodsSprites;
    public Sprite woodBackground;
    public GameObject background;

    //Current wall sprites
    public Sprite[] sprites;

    private float yBounds = -11f;
    private bool isTransition;
    private Sprite newBackground;

    void Start() {
        EventManager.StartListening(EventTypes.TRANSITION_START, StartTransition);
        EventManager.StartListening(EventTypes.TRANSITION_END, EndTransition);
        EventManager.StartListening(EventTypes.CHANGE_BACKGROUND, ChangeBackground);
    }

    private void ChangeBackground(object arg0) {
        background.GetComponent<SpriteRenderer>().sprite = newBackground;
    }

    private void StartTransition(object value)
    {
        sprites = null;
        EnvironmentModel newSprites = value as EnvironmentModel;
        newBackground = woodBackground;
        sprites = woodsSprites;
        isTransition = true;
    }

    private void EndTransition(object value)
    {
        EnvironmentModel newSprites = value as EnvironmentModel;
        newBackground = newSprites.backGround as Sprite;
        sprites = newSprites.wallSprites as Sprite[];
    }

    void FixedUpdate () {
            CheckWallPosition();
    }

    /// <summary>
    /// Loop though the wall lists to check if they need repositioning
    /// </summary>
    private void CheckWallPosition() {
        foreach (SpriteRenderer wall in bottomWalls) {
            if (wall.transform.position.y <= yBounds) {
                ResetWall(wall, topWalls[0].transform.position.y + topWalls[0].bounds.size.y);
            }
        }

        foreach (SpriteRenderer wall in topWalls) {
            if (wall.transform.position.y <= yBounds) {
                ResetWall(wall, bottomWalls[0].transform.position.y + bottomWalls[0].bounds.size.y);
            }
        }
    }

    /// <summary>
    /// Repositions the wall sprite back to the top and gives it a random sprite from the sprites array.
    /// </summary>
    /// <param name="wall"></param>
    private void ResetWall(SpriteRenderer wall, float yPos) {
        wall.transform.position = new Vector3(wall.transform.position.x, yPos, wall.transform.position.z);
        SetRandomSprite(wall);
    }

    private void SetRandomSprite(SpriteRenderer wall) {
        wall.sprite = sprites[Mathf.RoundToInt(Random.Range(0, sprites.Length))];
    }
}
