using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CanvasExplosionEffect : MonoBehaviour
{
    public static CanvasExplosionEffect instance;
    private Image image;

    void Awake()
    {
        instance = this;
        image = GetComponent<Image>();
    }


    public void Play()
    {
        image.DOFade(0.7f, 0f).OnComplete(() => image.DOFade(0f, .5f));
    }
}
