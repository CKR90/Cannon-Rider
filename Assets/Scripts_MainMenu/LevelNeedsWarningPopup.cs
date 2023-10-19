using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNeedsWarningPopup : MonoBehaviour
{
    public LevelBox popBox;
    public GameObject PointerTopBarCoin;
    public GameObject PointerTopBar;
    public GameObject PointerCoin;
    public GameObject PointerFruit;
    public void OpenPopup(LevelBox box)
    {
        popBox.levelData = box.levelData;
        popBox.ItemIcons = box.ItemIcons;

        popBox.Initialize();
        popBox.gameObject.SetActive(true);
        gameObject.SetActive(true);

        if(popBox.levelData.levelNeeds.CoinSize > User.info.coin)
        {
            PointerCoin.SetActive(true);
            PointerTopBarCoin.SetActive(true);
        }


        if (popBox.levelData.levelNeeds.itemType == ItemType.apple)
        {
            if (User.info.apple < popBox.levelData.levelNeeds.ItemSize)
            {
                PointerTopBar.SetActive(true);
                PointerFruit.SetActive(true);
            }
        }
        else if (popBox.levelData.levelNeeds.itemType == ItemType.pear)
        {
            if (User.info.pear < popBox.levelData.levelNeeds.ItemSize)
            {
                PointerTopBar.SetActive(true);
                PointerFruit.SetActive(true);
            }
        }
        else if (popBox.levelData.levelNeeds.itemType == ItemType.orange)
        {
            if (User.info.orange < popBox.levelData.levelNeeds.ItemSize)
            {
                PointerTopBar.SetActive(true);
                PointerFruit.SetActive(true);
            }
        }
    }
}
