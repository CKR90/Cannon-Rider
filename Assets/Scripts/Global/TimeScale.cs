using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    public static TimeScale Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void Set(float value)
    {
        Time.timeScale = value;
    }
}
