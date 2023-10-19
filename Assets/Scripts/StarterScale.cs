using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterScale : MonoBehaviour
{
    void Awake()
    {
        float ratio = (float)Screen.height / (float)Screen.width;
        if (ratio < 1.7f)
        {
            GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
        }
    }

}
