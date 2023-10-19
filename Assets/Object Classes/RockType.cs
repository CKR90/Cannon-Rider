using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RockType
{
    public string Name;
    public Rock RockName;
    [Min(3f)] public float SideLimitMin = 0;
    [Min(3f)] public float SideLimitMax = 0;
    public List<GameObject> rocks = new List<GameObject>();
}