using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class VehiclePanelScaler : MonoBehaviour
{
    public RectTransform gallery;
    public RectTransform upgrade;
    public RectTransform play;
    private void OnEnable()
    {
        float ratio = (float)Screen.height / (float)Screen.width;

        Vector2 size = upgrade.sizeDelta;
        Vector2 playpos = Vector2.zero;
        Vector2 galleryPos = Vector2.zero;
        float scale = 1f;

        if (ratio < 1.7f)
        {
            size.y = 520f;
            playpos.y = 10f;
            galleryPos.y = -200f;
            scale = 0.7f;
            play.localScale = Vector2.one * 0.55f;
        }
        else if (ratio > 1.8f)
        {
            size.y = 800f;
            playpos.y = 120f;
            galleryPos.y = -350f;
            play.localScale = Vector2.one;
        }
        else
        {
            size.y = 750f;
            playpos.y = 100f;
            galleryPos.y = -200f;
            play.localScale = Vector2.one;
        }

        gallery.localScale = Vector3.one * scale;
        gallery.anchoredPosition = galleryPos;
        upgrade.sizeDelta = size;
        play.anchoredPosition = playpos;
    }
}
