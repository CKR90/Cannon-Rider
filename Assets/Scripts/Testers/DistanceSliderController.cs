using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceSliderController : MonoBehaviour
{
    public TextMeshProUGUI SliderValueText;
    private Slider s;

    void Start()
    {
        s = GetComponent<Slider>();
    }

    void Update()
    {
        if(SliderValueText.text != s.value.ToString("0.0"))
        {
            SliderValueText.text = s.value.ToString("0.0");
        }
    }
}
