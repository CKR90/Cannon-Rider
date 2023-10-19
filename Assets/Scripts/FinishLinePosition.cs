using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLinePosition : MonoBehaviour
{
   
    void Start()
    {
        Vector3 pos = transform.position;
        pos.z = LevelDataTransfer.gamePlaySettings.roadDistance;

        transform.position = pos;
    }
}
