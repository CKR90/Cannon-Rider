using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FinishGameDarker : MonoBehaviour
{
    public static FinishGameDarker instance;

    public GameObject GameResults;

    private Image i;
    void Awake()
    {
        instance = this;
        i = GetComponent<Image>();
    }

    public void FinishGame()
    {
        i.DOFade(0.97f, 3f).OnComplete(() => GameResults.SetActive(true));
    }
}
