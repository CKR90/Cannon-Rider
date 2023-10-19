using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nGPU_RoadController : MonoBehaviour
{
    public Transform Vehicle;
    void Start()
    {
        
    }

    
    void Update()
    {
        if(Vector3.Distance(Vehicle.position, transform.position) > 30f)
        {
            transform.position = new Vector3(0f, 0f, transform.position.z + 30f);
        }
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
    }
}
