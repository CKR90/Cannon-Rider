using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetQualityLevel : MonoBehaviour
{
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(5 - index);
    }
}
