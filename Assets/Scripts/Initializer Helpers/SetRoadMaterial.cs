using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRoadMaterial : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<MeshRenderer>().sharedMaterial = LevelDataTransfer.mapSettings.roadMaterial;
        
    }
}
