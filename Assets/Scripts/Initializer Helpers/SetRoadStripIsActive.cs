using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRoadStripIsActive : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(LevelDataTransfer.mapSettings.EnableRoadStrip);
    }
}
