using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasketData
{
    public string Name;
    public BasketItem Item;
    public Mesh Mesh;
    public GameObject RBPrefab;
    public Sprite Icon;
    public Vector3 MeshScale;

    public List<Material> Materials;
    public List<Transform> PositionTransforms;

}