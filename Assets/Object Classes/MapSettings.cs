using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapSettings
{
    public bool EnableRoadStrip;
    public bool EnableSideStrip;
    public Material roadMaterial;
    public Material sideAreaMaterial;
    public Foliage foliageType;
    public Rock rockType;
    public Building buildingType;
    public Decal decalType;
}
