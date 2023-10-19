using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAreaActivator : MonoBehaviour
{
    public static GameAreaActivator Instance;

    [HideInInspector] public bool ControlEnabled = false;
    public void Enable(bool value)
    {
        ControlEnabled = value;
    }
    void Awake()
    {
        Instance = this;
    }
}
