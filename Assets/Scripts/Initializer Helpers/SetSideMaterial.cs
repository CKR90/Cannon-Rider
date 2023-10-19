using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSideMaterial : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<MeshRenderer>().sharedMaterial = LevelDataTransfer.mapSettings.sideAreaMaterial;
    }
}
