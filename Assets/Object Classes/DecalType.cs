using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecalType
{
    public string ListName;
    public Decal DecalName;
    public bool randomRotation;
    [Min(0f)] public float SideLimitMin = 0;
    [Min(0f)] public float SideLimitMax = 0;
    public List<GameObject> decals = new List<GameObject>();
}