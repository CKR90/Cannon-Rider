using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorBlink : MonoBehaviour
{
    private Image i;

    float timer = 0f;

    void Start()
    {
        i = GetComponent<Image>();
    }

    private void Update()
    {
        Blink();
    }
    private void Blink()
    {
        timer += Time.deltaTime;
        if(timer >= 0.5f)
        {
            timer = 0f;
            i.enabled = !i.enabled;
        }
    }

    public void ResetCursorBlink()
    {
        timer = 0f;
        i.enabled = true;
    }
}
