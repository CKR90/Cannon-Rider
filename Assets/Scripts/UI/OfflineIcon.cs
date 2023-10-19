using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineIcon : MonoBehaviour
{

    private float timer = 0f;
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if(InternetCheck.internet)
        {
            if(img.enabled) img.enabled = false;
            return;
        }

        if(!InternetCheck.internet)
        {
            if(!img.enabled) img.enabled = true;

            timer += Time.deltaTime;
            if (timer >= 1.8f) timer = .2f;

            Color c = img.color;
            c.a = Mat.sin(timer * 90f);

            img.color = c;
        }
    }
}
