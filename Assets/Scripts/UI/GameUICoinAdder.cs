using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameUICoinAdder : MonoBehaviour
{
    public static GameUICoinAdder Instance;


    public RectTransform CoinImg;
    public TextMeshProUGUI CoinText;


    private void Awake()
    {
        Instance = this;
    }
    public void AddCoin(Canvas parentCanvas, Vector3 CoinWorldPosition, int coinValue)
    {
        RectTransform rt = Instantiate(CoinImg.gameObject, transform).GetComponent<RectTransform>();

        TextMeshProUGUI tm = rt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tm.SetText("+" + coinValue.ToString());

        rt.gameObject.SetActive(true);

        Vector3 pos = WorldToRectPoint(parentCanvas, CoinWorldPosition);
        rt.anchoredPosition3D = pos;

        pos.x += Random.Range(-50f, 50f);
        pos.y += Random.Range(100f, 150f);

        rt.localScale = Vector3.zero;

        rt.DOScale(1f, .2f);

        rt.DOAnchorPos3D(pos, Random.Range(0.3f, 0.6f)).SetEase(Ease.OutSine).OnComplete(() => 
        {
            rt.DOAnchorPos3D(Vector3.zero, .35f).SetEase(Ease.Linear).OnComplete(() =>
            {
                int coin = GameDatabase.Instance.IncreaseCoin(coinValue);
                CoinText.SetText(coin.ToString());
                Destroy(rt.gameObject);
                SoundController.instance.Play(SFXList.CoinSound);
            });
        });

        

        DailyMissionDatabase.AddDailyData(DailyItem.Coin, coinValue);
    }


    private Vector3 WorldToRectPoint(Canvas parentCanvas,Vector3 point)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(point);
        RectTransform r = Instantiate(CoinImg.gameObject, parentCanvas.transform).GetComponent<RectTransform>();
        r.position = pos;
        r.transform.SetParent(transform);
        Vector3 localPos = r.anchoredPosition3D;
        Destroy(r.gameObject);
        return localPos;
    }

    public void ResetCoinText()
    {
        CoinText.SetText("0");
    }
}
