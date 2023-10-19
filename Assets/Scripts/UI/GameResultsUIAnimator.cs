using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.UI;

public class GameResultsUIAnimator : MonoBehaviour
{

    public List<RectTransform> items = new List<RectTransform>();
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI CoinText;
    public GameObject ContinueButton;
    public Image BasketItemImage;
    public TextMeshProUGUI BasketItemSize;
    public Sprite Apple;
    public Sprite Pear;
    public Sprite Orange;

    private List<Vector3> defaultPoses = new List<Vector3>();
    private List<Vector3> defaultScales = new List<Vector3>();

    private int completedCountSize = 0;
    private int killComponentSize = 0;
    void Start()
    {
        foreach (var i in items)
        {
            defaultPoses.Add(i.anchoredPosition3D);
            defaultScales.Add(i.localScale);

            i.anchoredPosition3D = Vector3.zero;
            i.localScale = Vector3.zero;
        }

        TimeSpan time = TimeSpan.FromSeconds(PlayTimeCounter.Instance.TimeToFinish);
        TimeText.SetText(time.ToString(@"mm\:ss\.fff"));
        CoinText.SetText(GameDatabase.Instance.GetCoinSize().ToString());

        if(LevelDataTransfer.gamePlaySettings.basketEnable)
        {
            switch (LevelDataTransfer.gamePlaySettings.basketItem)
            {
                case BasketItem.Apple: BasketItemImage.sprite = Apple; break;
                case BasketItem.Pear: BasketItemImage.sprite = Pear; break;
                case BasketItem.Orange: BasketItemImage.sprite = Orange; break;
            }
            BasketItemSize.SetText(GameDatabase.Instance.GetBasketItem().ToString());
        }



        ShowResult();
    }

    public void ShowResult()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].DOAnchorPos3D(defaultPoses[i], .5f);
            items[i].DOScale(defaultScales[i], .5f);
        }
        Invoke("EnebleKillCounters", 1f);
    }

    private void EnebleKillCounters()
    {
        foreach (var i in items)
        {
            if (i.transform.TryGetComponent<GameResultKillCounter>(out GameResultKillCounter k))
            {
                k.Play(CountComplete);
                killComponentSize++;
            }
        }
    }

    private void CountComplete()
    {
        completedCountSize++;

        if (completedCountSize >= killComponentSize)
        {
            if(LevelDataTransfer.gamePlaySettings.basketEnable)
            {
                EnableBasketItem();
            }
            else
            {
                ContinueButton.SetActive(true);
            } 
        }
    }

    private void EnableBasketItem()
    {
        BasketItemSize.gameObject.GetComponent<RectTransform>().DOScale(1f, .5f).OnComplete(() =>
        { 
            SoundController.instance.Play(SFXList.Crash_Bubble3);
            ContinueButton.SetActive(true);
        });
    }
}
