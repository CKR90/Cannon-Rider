using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggyBombAreaParticle : MonoBehaviour
{
    private bool bigger = false;
    private float sin = 45f;
    void Update()
    {
        Vector3 scale = transform.localScale;

        if (!bigger && scale.x > .6f) bigger = true;

        if (bigger)
        {
            if (scale.x < .1f)
            {
                Destroy(gameObject);
            }
        }
        sin += Time.deltaTime * 100f;
        transform.localScale = Vector3.one * Mat.sin(sin);
    }
}
