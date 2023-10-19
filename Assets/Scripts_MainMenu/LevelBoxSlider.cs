using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoxSlider : MonoBehaviour
{
    public RectTransform rt;
    private int packCheck = 0;
    private bool enable = false;

    private void Start()
    {
        Invoke("EnableThis", 1f);
    }



    void FixedUpdate()
    {
        if (!enable) return;
        if (LevelDataGenerator.Instance.levels.Count <= 0) return;
        if (LevelDataGenerator.Instance.packs == null || LevelDataGenerator.Instance.packs.Count <= 0) return;

        PackEnableCheck();
    }

    private void EnableThis()
    {
        enable = true;
    }

    private void PackEnableCheck()
    {
        for (int i = 0; i < 5; i++)
        {
            packCheck++;
            packCheck = packCheck % LevelDataGenerator.Instance.packs.Count;

            float screenH = rt.anchoredPosition.y - packCheck * 1720f;

            if(screenH > Screen.height * 1.5f || screenH < -Screen.height * 1.5f)
            {
                LevelDataGenerator.Instance.packs[packCheck].SetActive(false);
            }
            else
            {
                LevelDataGenerator.Instance.packs[packCheck].SetActive(true);
            }
        }
    }
}
