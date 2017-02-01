using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StoreUI : MonoBehaviour {
    public GameObject confirmPopUp;
    public GameObject notEnoughFunds;
    public Button[] storeButtons;

    public Text coins;
    
    private int coinAmountFromSave;
    private int currentCoinAmount;
    private bool matchFound;

    //store current item being bought
    private StoreItemModel itemInCart;

    private void Start() {
        //Disable confirm purchase popup
        confirmPopUp.SetActive(false);
        notEnoughFunds.SetActive(false);
        itemInCart = null;

        //Show amount of coins from savegame
        coinAmountFromSave = SaveLoadController.GetInstance().GetPlayer().GetTotalCoins();
        currentCoinAmount = coinAmountFromSave;
        coins.text = "Coins: " + currentCoinAmount.ToString();
    }

    private void PurchaseStarted(GameObject item) {
        //Check price and current amount
        StoreItemModel itemToPurchase = item.GetComponent<StoreItemModel>();
        if (itemToPurchase.goldPrice <= currentCoinAmount)
        {
            itemInCart = itemToPurchase; //save item player wants to purchase (Cart)
            confirmPopUp.SetActive(true);
        }
        else {
            notEnoughFunds.SetActive(true);
        }
    }

    public void PurchaseConfirmed() {
        //Confirm purchase and adjust coin amount accordigly
        currentCoinAmount -= itemInCart.goldPrice;
        coins.text = "Coins: " + currentCoinAmount.ToString();
        confirmPopUp.SetActive(false);
        EventManager.TriggerEvent(EventTypes.PURCHASE_CONFIRMED);
        SaveLoadController.GetInstance().GetPlayer().AddTotalCoins(-itemInCart.goldPrice); //substract coins from player
        SaveLoadController.GetInstance().GetPlayer().AddUnlockedItem(itemInCart.itemID); //unlock skin for player
        SetSkinActive(itemInCart.itemID); // Set skin active. This also saves the game

        // Send event data
        // This is very ugly but ok for now
        switch (itemInCart.skinName) {
            case "Vladimir":
                GooglePlayHelper.GetInstance().ReportEvent(GPGSConstant.event_store_default_skin_bought, 1);
                break;
            default:
                break;
        }

        itemInCart = null;
    }

    public void PurchaseCanceled() {
        //Return to store
        confirmPopUp.SetActive(false);
        notEnoughFunds.SetActive(false);
        itemInCart = null;
    }

    public void PurchaseItem(GameObject item) {
        //Quick hack to set a skin active
        List<int> unlockedItemList = SaveLoadController.GetInstance().GetPlayer().GetUnlockedItems();

        foreach (int id in unlockedItemList)
        {
            if (item.GetComponent<StoreItemModel>().itemID == id)
            {
                SetSkinActive(item.GetComponent<StoreItemModel>().itemID);
                Debug.Log("Skin: " + item.GetComponent<StoreItemModel>().itemID + " is active");
                matchFound = true;
            }
        }

        if (!matchFound) {
            PurchaseStarted(item);
            Debug.Log("Purchase: " + item.GetComponent<StoreItemModel>().itemID + " started");
        }

        matchFound = false;
    }

    private void SetSkinActive(int id) {
        SaveLoadController.GetInstance().GetPlayer().SetActiveSkinID(id);
        EventManager.TriggerEvent(EventTypes.NEW_SKIN_ACTIVE);
        GooglePlayHelper.GetInstance().SaveGame();
    }
}
