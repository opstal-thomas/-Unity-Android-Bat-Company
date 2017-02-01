using UnityEngine;
using System.Collections;

public class GPMPTileManager : MonoBehaviour {
    public SpriteRenderer[] bottomTiles, topTiles;
    
    public Sprite[] sprites;

    private float yBounds = -11f;
    

    void FixedUpdate() {
        CheckTilePosition();
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
    }
}
