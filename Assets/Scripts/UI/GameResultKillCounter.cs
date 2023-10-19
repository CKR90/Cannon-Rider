using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultKillCounter : MonoBehaviour
{
    public TextMeshProUGUI t;
    public RectTransform Shine;
    public RectTransform Coin;
    public TextMeshProUGUI CoinText;

    private int KillCount = 0;

    private bool play = false;
    private float timer = 0f;
    private float limit = 0f;
    private int counter = 0;

    private Action callback;

    void Start()
    {
        limit = UnityEngine.Random.Range(.1f, .2f);
        t.SetText("0");
    }
    void Update()
    {
        if (!play) return;

        timer += Time.deltaTime;

        if (timer >= limit)
        {
            timer = 0f;
            if (counter < KillCount)
            {
                counter++;
                t.SetText(counter.ToString());
                SoundController.instance.Play((SFXList)UnityEngine.Random.Range((int)SFXList.Crash_Bubble1, (int)SFXList.Crash_Bubble3 + 1));
                Shine.DOScale(1f, 0.05f).OnComplete(() => Shine.DOScale(0f, 0.045f));
            }
            else
            {
                play = false;
                callback();

                //Invoke("AddToCoin", UnityEngine.Random.Range(0.1f, 1.5f));
            }
        }
    }

    private void AddToCoin()
    {
        GetComponent<RectTransform>().DOAnchorPos3D(Coin.anchoredPosition3D, .3f).OnComplete(() =>
        {
            GetComponent<RectTransform>().localScale = Vector3.zero;
            SoundController.instance.Play(SFXList.CoinSound);
            int value = int.Parse(CoinText.text);
            value += KillCount * 10;
            CoinText.SetText(value.ToString());
        });
    }

    public void Play(Action callback)
    {
        play = true;
        this.callback = callback;

        switch (gameObject.name)
        {
            case "BG Banana": KillCount = GameDatabase.Instance.GetKilledBanana(); break;
            case "BG Eggy": KillCount = GameDatabase.Instance.GetKilledEggy(); break;
            case "BG Orange": KillCount = GameDatabase.Instance.GetKilledOrange(); break;
            case "BG Pear": KillCount = GameDatabase.Instance.GetKilledPear(); break;
        }
    }
}
