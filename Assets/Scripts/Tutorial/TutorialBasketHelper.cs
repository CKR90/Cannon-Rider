using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBasketHelper : MonoBehaviour
{
    public RectTransform fruitPointer;
    public Image Darker;
    private TutorialController controller;


    private Vector3 pos;

    private Vector3 start;

    void Start()
    {
        controller = GetComponent<TutorialController>();
        pos = fruitPointer.anchoredPosition;
        start = pos;
        start.x += 1000f;
        fruitPointer.anchoredPosition = start;
        fruitPointer.gameObject.SetActive(true);
        Darker.gameObject.SetActive(true);

        fruitPointer.anchoredPosition = pos;
    }

    public void ClosePointer()
    {
        Darker.DOFade(0f, 1f).OnComplete(() => Destroy(Darker.gameObject));

        fruitPointer.DOAnchorPos3D(start, 1f).OnComplete(() =>
        {
            //PlayerPrefs.SetInt(TutorialType.FruitPointer.ToString(), 1);
            fruitPointer.gameObject.SetActive(false);
            controller.TapToPlayScreen.SetActive(true);
            Destroy(gameObject);
        });
    }
}
