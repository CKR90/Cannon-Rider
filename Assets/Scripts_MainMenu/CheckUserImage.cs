using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckUserImage : MonoBehaviour
{
    public Sprite defaultIcon;

    private float timer = 0f;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        if(User.sprite != null )
        {
            if(image.sprite != User.sprite)
            {
                image.sprite = User.sprite;
            }
        }
        else
        {
            if(image.sprite != defaultIcon)
            {
                image.sprite = defaultIcon;
            }
        }
    }
}
