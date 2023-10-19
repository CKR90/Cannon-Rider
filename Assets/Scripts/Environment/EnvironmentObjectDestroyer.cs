using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObjectDestroyer : MonoBehaviour
{
    [HideInInspector] public Transform Vehicle;
    [HideInInspector] public float BackSideDestroyDistance = 0f;

    void FixedUpdate()
    {
        if(Vehicle.position.z - transform.position.z > BackSideDestroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
