using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeTrailController : MonoBehaviour
{
    public List<GameObject> trails = new List<GameObject>();
    public void SetToggle(bool toggle)
    {
        foreach (GameObject trail in trails)
        {
            trail.SetActive(toggle);
        }
    }
}
