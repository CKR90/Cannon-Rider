using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyAwardApply : MonoBehaviour
{
    public static DailyAwardApply Instance;

    public RectTransform chest;
    public RectTransform Coin;
    public TextMeshProUGUI CoinText;
    public GameObject ContinueButton;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        transform.SetAsLastSibling();
    }

    public void PayDailyAwardAndUpdateDatabases()
    {
        SoundController.instance.Play(SFXList.VictoryAward);

        ContinueButton.SetActive(false);
        Coin.localScale = Vector3.zero;
        chest.localScale = Vector3.zero;

        int coinSize = Random.Range(3, 11) * 1000;
        if (coinSize > 6000) coinSize = Random.Range(3, 11) * 1000;
        if (coinSize > 8000) coinSize = Random.Range(3, 11) * 1000;

        CoinText.SetText(coinSize.ToString());



        User.info.coin += coinSize;
        GameDatabase.Instance.SaveLocalData();
        DBManager.Instance.UpdateItem("coin", User.info.coin);


        gameObject.SetActive(true);

        chest.DOScale(1f, 2f).SetEase(Ease.OutBounce);

        Invoke("ScaleCoin", 1f);
        Invoke("OpenButton", 3f);
    }

    private void ScaleCoin()
    {
        Coin.DOScale(1f, 2f).SetEase(Ease.OutBounce);
    }

    private void OpenButton()
    {
        ContinueButton.SetActive(true);
    }

    public void ButtonClickEvent()
    {
        SoundController.instance.Play(SFXList.ButtonClick);
        ContinueButton.SetActive(false);
        Coin.localScale = Vector3.zero;
        chest.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }


    void Update()
    {
        
    }
}
