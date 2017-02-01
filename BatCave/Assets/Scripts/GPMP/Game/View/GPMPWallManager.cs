using UnityEngine;
using System.Collections;

public class GPMPWallManager : MonoBehaviour {
    public SpriteRenderer[] bottomWalls;
    public SpriteRenderer[] topWalls;

    public Sprite[] sprites;

    private float yBounds = -11f;

    void FixedUpdate() {
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
