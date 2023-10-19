using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKill : MonoBehaviour
{
    private void Start()
    {
        //Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        //rb.mass = 10f;
    }
    void Update()
    {
        if(transform.position.z < -100f) Destroy(gameObject);
    }
}
