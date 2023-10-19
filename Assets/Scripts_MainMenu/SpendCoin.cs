using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpendCoin : MonoBehaviour
{
    public static SpendCoin Instance;

    private Image img;
    private TextMeshProUGUI txt;

    private RectTransform rt;
    private Vector2 defaultPos;

    private Vector2 target;

    void Awake()
    {
        Instance = this;
        img = GetComponent<Image>();
        txt = transform.GetComponentInChildren<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
        defaultPos = rt.anchoredPosition;

        target = new Vector2(defaultPos.x + 100f, defaultPos.y -100f);
    }

    public void SpendAnim(int value)
    {
        DOTween.Kill(rt);
        value = Mathf.Abs(value);
        txt.text = (-value).ToString();
        rt.anchoredPosition = defaultPos;

        img.enabled = true;
        txt.enabled = true;

        rt.DOAnchorPos(target, 1f).SetEase(Ease.OutCirc).OnComplete(() =>
        {

            img.enabled = false;
            txt.enabled = false;
        });
        
    }
}
