using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRoadLinesIsActive : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(LevelDataTransfer.mapSettings.EnableSideStrip);
    }
}
