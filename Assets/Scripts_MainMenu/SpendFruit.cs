using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpendFruit : MonoBehaviour
{
    public static SpendFruit Instance;

    public List<FruitSpendData> Fruits = new List<FruitSpendData>();

    
    void Awake()
    {
        Instance = this;
        foreach(var f in Fruits)
        {
            f.defaultPos = f.rect.anchoredPosition;
            f.targetPos = new Vector2(f.defaultPos.x + 100f, f.defaultPos.y - 100f);
        }
    }


    public void Spend(ItemType item, int size)
    {
        FruitSpendData f = Fruits.Find(x => x.fruit == item);
        if (f == null) return;

        DOTween.Kill(f.rect);

        f.rect.anchoredPosition = f.defaultPos;
        f.text.SetText("-" + size);
        f.rect.gameObject.SetActive(true);
        f.rect.DOAnchorPos(f.targetPos, 1f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            f.rect.gameObject.SetActive(false);
        });
    }

    [System.Serializable] public class FruitSpendData
    {
        public ItemType fruit;
        public RectTransform rect;
        public TextMeshProUGUI text;
        [HideInInspector] public Vector2 defaultPos;
        [HideInInspector] public Vector2 targetPos;
    }
}
