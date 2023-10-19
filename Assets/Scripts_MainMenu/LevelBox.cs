using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelBox : MonoBehaviour
{

    public LevelData levelData;
    public LevelBoxObjects objects;
    public List<ItemIcon> ItemIcons;
    [HideInInspector] public GameObject BackToMapsButton;
    [HideInInspector] public GameObject BackToMainMenuButton;
    [HideInInspector] public GameObject VehiclePanel;
    [HideInInspector] public GameObject PackParent;

    private Button button;
    private bool showAd = false;
    private bool award = false;

    [HideInInspector] public bool IamNextLevel = false;
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        button = GetComponent<Button>();

        objects.LevelNumber.SetText((levelData.gamePlaySettings.levelIndex + 1).ToString());

        SetBasketImage();
        SetAwardImage();


        if (!award)
        {
            GetComponent<Image>().color = Color.white;
            GetComponent<Image>().sprite = levelData.levelUI.levelTexture;
        }
        if (LevelAlreadyOpen()) { }
        else if (LevelIsNext()) { }
        else { LevelClosed(); }
    }
    private bool LevelAlreadyOpen()
    {
        if (User.info.lastcompletedlevelindex >= levelData.gamePlaySettings.levelIndex)
        {
            GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 1f);
            objects.CoinObject.SetActive(false);
            objects.ItemObject.SetActive(false);
            objects.AdObject.SetActive(false);
            objects.LockerObject.SetActive(false);

            if (User.info.times.Count > levelData.gamePlaySettings.levelIndex)
            {
                TimeSpan time = TimeSpan.FromSeconds(User.info.times[levelData.gamePlaySettings.levelIndex]);
                objects.timeText.SetText(time.ToString(@"mm\:ss\.fff"));
                objects.timeText.gameObject.SetActive(true);
            }
            else
            {
                objects.timeText.gameObject.SetActive(false);
            }
            IamNextLevel = false;
            return true;
        }
        return false;
    }
    private bool LevelIsNext()
    {
        if (User.info.lastcompletedlevelindex == levelData.gamePlaySettings.levelIndex - 1)
        {
            objects.LockerObject.SetActive(false);
            objects.PlayImage.gameObject.SetActive(true);


            ItemType levelItem = levelData.levelNeeds.itemType;
            if(User.info.lastpaidlevelindex < levelData.gamePlaySettings.levelIndex)
            {
                if (levelData.levelNeeds.CoinSize > 0)
                {
                    objects.CoinObject.SetActive(true);
                    objects.CoinText.SetText(levelData.levelNeeds.CoinSize.ToString());
                }
                else
                {
                    objects.CoinObject.SetActive(false);
                    objects.CoinText.SetText("");
                }
                if (levelData.levelNeeds.ItemSize > 0)
                {
                    objects.ItemObject.SetActive(true);
                    objects.ItemObject.GetComponent<Image>().sprite = ItemIcons.Find(x => x.itemType == levelItem).sprite;
                    objects.ItemText.SetText(levelData.levelNeeds.ItemSize.ToString());
                }
                else
                {
                    objects.ItemObject.SetActive(false);
                    objects.ItemText.SetText("");
                }
            }
            else
            {
                objects.CoinObject.SetActive(false);
                objects.ItemObject.SetActive(false);
            }

            IamNextLevel = true;
            return true;
        }
        return false;
    }
    private void LevelClosed()
    {
        objects.AdObject.SetActive(false);
        objects.LockerObject.SetActive(true);
        objects.CoinObject.SetActive(false);
        objects.ItemObject.SetActive(false);
        button.enabled = false;
        IamNextLevel = false;
    }
    public void ClickEvent()
    {
        SoundController.instance.Play(SFXList.ButtonClick);

        bool itemsizeEnough = false;

        if (User.info.lastpaidlevelindex < levelData.gamePlaySettings.levelIndex)
        {
            if (levelData.levelNeeds.itemType == ItemType.apple)
            {
                if (User.info.apple >= levelData.levelNeeds.ItemSize) itemsizeEnough = true;
            }
            else if (levelData.levelNeeds.itemType == ItemType.pear)
            {
                if (User.info.pear >= levelData.levelNeeds.ItemSize) itemsizeEnough = true;
            }
            else if (levelData.levelNeeds.itemType == ItemType.orange)
            {
                if (User.info.orange >= levelData.levelNeeds.ItemSize) itemsizeEnough = true;
            }
            else itemsizeEnough = true;



            if (itemsizeEnough)
            {
                if (User.info.coin >= levelData.levelNeeds.CoinSize)
                {
                    if(levelData.levelNeeds.ItemSize > 0)
                    {
                        SpendFruit.Instance.Spend(levelData.levelNeeds.itemType, levelData.levelNeeds.ItemSize);
                    }
                    if (levelData.levelNeeds.CoinSize > 0)
                    {
                        SpendCoin.Instance.SpendAnim(levelData.levelNeeds.CoinSize);
                    }

                    if(levelData.levelNeeds.ItemSize > 0 || levelData.levelNeeds.CoinSize > 0)
                    {
                        SoundController.instance.Play(SFXList.CoinFalling);
                    }

                    GameDatabase.Instance.PayLevelNeeds(levelData.levelNeeds, levelData.gamePlaySettings.levelIndex);
                    objects.CoinObject.SetActive(false);
                    objects.ItemObject.SetActive(false);
                    Join();
                }
                else
                {
                    SoundController.instance.Play(SFXList.Denied);

                    objects.Popup.GetComponent<LevelNeedsWarningPopup>().OpenPopup(this);
                    objects.PopupText.SetText("You don't have enough coins!\nEarn from old levels");
                    //objects.Popup.SetActive(true);
                    return;
                }
            }
            else
            {
                if (User.info.coin < levelData.levelNeeds.CoinSize)
                {
                    SoundController.instance.Play(SFXList.Denied);

                    objects.Popup.GetComponent<LevelNeedsWarningPopup>().OpenPopup(this);
                    objects.PopupText.SetText("You don't have enough coins\nand fruits!\nEarn from old levels");
                    //objects.Popup.SetActive(true);
                }
                else
                {
                    SoundController.instance.Play(SFXList.Denied);

                    objects.Popup.GetComponent<LevelNeedsWarningPopup>().OpenPopup(this);
                    objects.PopupText.SetText("You don't have enough fruits!\nEarn from old levels");
                    //objects.Popup.SetActive(true);
                }
                return;
            }
        }
        else
        {
            if(levelData.gamePlaySettings.levelIndex == 0 && User.info.lastcompletedlevelindex < 0)
            {
                JoinDirect();
            }
            else
            {
                Join();
            }
            
        }
    }
    private void Join()
    {
        TopBarData.Instance.SetData();
        BackToMapsButton.SetActive(true);
        BackToMainMenuButton.SetActive(false);
        VehiclePanel.SetActive(true);
        PackParent.gameObject.SetActive(false);

        levelData.planeEvent = LevelDataGenerator.Instance.levels[Mathf.Max(User.info.lastcompletedlevelindex, levelData.gamePlaySettings.levelIndex)].planeEvent;
        LevelDataTransfer.SetLevelData(levelData);

        AwardShows.Instance.CheckAwardShow(levelData.levelAwards, this);

        if (IamNextLevel)
        {
            objects.PlayImage.gameObject.SetActive(true);
        }
    }
    private void JoinDirect()
    {
        TopBarData.Instance.SetData();
        BackToMapsButton.SetActive(true);
        BackToMainMenuButton.SetActive(false);
        VehiclePanel.SetActive(true);
        PackParent.gameObject.SetActive(false);

        levelData.planeEvent = LevelDataGenerator.Instance.levels[Mathf.Max(User.info.lastcompletedlevelindex, levelData.gamePlaySettings.levelIndex)].planeEvent;
        LevelDataTransfer.SetLevelData(levelData);

        AwardShows.Instance.CheckAwardShow(levelData.levelAwards, this);

        if (IamNextLevel)
        {
            objects.PlayImage.gameObject.SetActive(true);
        }
        LevelDataGenerator.Instance.LoadMap();
    }

    private void SetBasketImage()
    {
        if (levelData.gamePlaySettings.basketEnable)
        {
            switch (levelData.gamePlaySettings.basketItem)
            {
                case BasketItem.Apple: objects.BasketImage.sprite = LevelDataGenerator.Instance.sprites.BasketApple; break;
                case BasketItem.Orange: objects.BasketImage.sprite = LevelDataGenerator.Instance.sprites.BasketOrange; break;
                case BasketItem.Pear: objects.BasketImage.sprite = LevelDataGenerator.Instance.sprites.BasketPear; break;
            }

            objects.BasketImage.gameObject.SetActive(true);
        }
        else
        {
            objects.BasketImage.gameObject.SetActive(false);
        }
    }
    private void SetAwardImage()
    {
        if (User.info.lastcompletedlevelindex >= levelData.gamePlaySettings.levelIndex) return;

        LevelAwards x = levelData.levelAwards;

        if (x.Booster && !PlayerPrefs.HasKey("Award_Booster"))
        {
            objects.AwardImage.sprite = LevelDataGenerator.Instance.sprites.Booster;
            objects.AwardImage.gameObject.SetActive(true);
            levelData.levelNeeds.CoinSize = 0;
            levelData.levelNeeds.ItemSize = 0;
            award = true;
        }
        else if (x.Protector && !PlayerPrefs.HasKey("Award_Protector"))
        {
            objects.AwardImage.sprite = LevelDataGenerator.Instance.sprites.Protector;
            objects.AwardImage.gameObject.SetActive(true);
            levelData.levelNeeds.CoinSize = 0;
            levelData.levelNeeds.ItemSize = 0;
            award = true;
        }
        else if (x.Coin && !PlayerPrefs.HasKey("Award_Coin"))
        {
            objects.AwardImage.sprite = LevelDataGenerator.Instance.sprites.Coin;
            objects.AwardImage.gameObject.SetActive(true);
            levelData.levelNeeds.CoinSize = 0;
            levelData.levelNeeds.ItemSize = 0;
            award = true;
        }
        else if (x.PirateBomb && !PlayerPrefs.HasKey("Award_Pirate"))
        {
            objects.AwardImage.sprite = LevelDataGenerator.Instance.sprites.PirateBomb;
            objects.AwardImage.gameObject.SetActive(true);
            levelData.levelNeeds.CoinSize = 0;
            levelData.levelNeeds.ItemSize = 0;
            award = true;
        }
        else if (x.HomingMissile && !PlayerPrefs.HasKey("Award_Missile"))
        {
            objects.AwardImage.sprite = LevelDataGenerator.Instance.sprites.Missile;
            objects.AwardImage.gameObject.SetActive(true);
            levelData.levelNeeds.CoinSize = 0;
            levelData.levelNeeds.ItemSize = 0;
            award = true;
        }
        else if (x.TNT && !PlayerPrefs.HasKey("Award_TNT"))
        {
            objects.AwardImage.sprite = LevelDataGenerator.Instance.sprites.TNT;
            objects.AwardImage.gameObject.SetActive(true);
            levelData.levelNeeds.CoinSize = 0;
            levelData.levelNeeds.ItemSize = 0;
            award = true;
        }
        else if (x.Basket && !PlayerPrefs.HasKey("Award_Basket"))
        {
            objects.AwardImage.sprite = LevelDataGenerator.Instance.sprites.Basket;
            objects.AwardImage.gameObject.SetActive(true);
            levelData.levelNeeds.CoinSize = 0;
            levelData.levelNeeds.ItemSize = 0;
            award = true;
        }
        else
        {
            objects.AwardImage.gameObject.SetActive(false);
            award = false;
        }
    }
}
