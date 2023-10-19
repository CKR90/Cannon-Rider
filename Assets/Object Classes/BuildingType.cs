using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingType
{
    public string Name;
    public Building BuildingName;
    [Min(3f)] public float SideLimitMin = 0;
    [Min(3f)] public float SideLimitMax = 0;
    public List<GameObject> buildings = new List<GameObject>();
}