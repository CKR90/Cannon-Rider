using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMoveSoundController : MonoBehaviour
{
    public WheelCollider anyWheel;

    private Rigidbody rb;
    private AudioSource a;
    void Start()
    {
        a = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        a.volume = Mathf.Clamp(rb.velocity.magnitude / 2f, 0f, 1f);
    }
}
