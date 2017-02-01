using UnityEngine;

public class SpawnPointModel : MonoBehaviour {
    
    private GameItem item;
    private bool isTaken;
    
    public void SetItem(GameItem item) {
        this.item = item;
        isTaken = true;
    }

    // reserve slot but leave obstacle and pickup empty
    public void reserveSlot() {
        isTaken = true;
    }

    public bool IsSlotTaken() {
        return isTaken;
    }

    public GameItem GetItem() {
        return this.item;
    }

    public void Reset() {
        this.item = null;
        this.isTaken = false;
    }
}
