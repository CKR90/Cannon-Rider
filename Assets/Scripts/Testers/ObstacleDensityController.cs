using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleDensityController : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    private Slider s;
    void Start()
    {
        s = GetComponent<Slider>();
    }


    void Update()
    {
        if(numberText.text != ((int)s.value).ToString())
        {
            numberText.text = ((int)s.value).ToString();
        }
    }
}
