using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoliageType
{
    public string Name;
    public Foliage FoliageName;
    [Min(3f)] public float SideLimitMin = 0;
    [Min(3f)] public float SideLimitMax = 0;
    public List<GameObject> foliages = new List<GameObject>();
}