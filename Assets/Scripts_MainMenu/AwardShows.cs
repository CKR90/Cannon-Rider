using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AwardShows : MonoBehaviour
{
    public static AwardShows Instance;

    public Image AwardImage;
    public GameObject ContinueButton;
    public TextMeshProUGUI DescriptionText;
    public Sprite Booster;
    public Sprite Coin;
    public Sprite Missile;
    public Sprite Pirate;
    public Sprite TNT;
    public Sprite Protector;
    public Sprite Basket;
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        transform.SetAsLastSibling();
    }

    public void CheckAwardShow(LevelAwards levelAwards, LevelBox box)
    {
        if (levelAwards.Coin || levelAwards.HomingMissile || levelAwards.PirateBomb || levelAwards.TNT || levelAwards.Booster || levelAwards.Protector || levelAwards.Basket)
        {
            if (levelAwards.Coin)
            {
                if (!PlayerPrefs.HasKey("Award_Coin"))
                {
                    PlayerPrefs.SetInt("Award_Coin", 1);
                    AwardImage.sprite = Coin;
                    DescriptionText.SetText("Coin Box");
                }
                else return;
            }
            else if (levelAwards.Booster)
            {
                if (!PlayerPrefs.HasKey("Award_Booster"))
                {
                    PlayerPrefs.SetInt("Award_Booster", 1);
                    AwardImage.sprite = Booster;
                    DescriptionText.SetText("Booster");
                }
                else return;
            }
            else if (levelAwards.PirateBomb)
            {
                if (!PlayerPrefs.HasKey("Award_Pirate"))
                {
                    PlayerPrefs.SetInt("Award_Pirate", 1);
                    AwardImage.sprite = Pirate;
                    DescriptionText.SetText("Pirate Bomb");
                }
                else return;
            }
            else if (levelAwards.TNT)
            {
                if (!PlayerPrefs.HasKey("Award_TNT"))
                {
                    PlayerPrefs.SetInt("Award_TNT", 1);
                    AwardImage.sprite = TNT;

                    DescriptionText.SetText("TNT");
                }
                else return;
            }
            else if (levelAwards.HomingMissile)
            {
                if (!PlayerPrefs.HasKey("Award_Missile"))
                {
                    PlayerPrefs.SetInt("Award_Missile", 1);
                    AwardImage.sprite = Missile;
                    DescriptionText.SetText("Homing Missile");
                }
                else return;
            }
            else if (levelAwards.Protector)
            {
                if (!PlayerPrefs.HasKey("Award_Protector"))
                {
                    PlayerPrefs.SetInt("Award_Protector", 1);
                    AwardImage.sprite = Protector;
                    DescriptionText.SetText("Protector");
                }
                else return;
            }
            else if (levelAwards.Basket)
            {
                if (!PlayerPrefs.HasKey("Award_Basket"))
                {
                    PlayerPrefs.SetInt("Award_Basket", 1);
                    AwardImage.sprite = Basket;
                    DescriptionText.SetText("Fruit Basket");
                }
                else return;
            }

            box.objects.AwardImage.gameObject.SetActive(false);
            box.gameObject.GetComponent<Image>().color = Color.white;
            box.gameObject.GetComponent<Image>().sprite = box.levelData.levelUI.levelTexture;

            gameObject.SetActive(true);

            SoundController.instance.Play(SFXList.VictoryAward);
            AwardImage.gameObject.GetComponent<RectTransform>().DOScale(1f, 2f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                ContinueButton.SetActive(true);
            });
        }
    }

    public void ResetAwardImage() 
    {
        ContinueButton.SetActive(false);
        DOTween.Clear(true);
        AwardImage.gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}
