using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireSlipSound : MonoBehaviour
{
    public AudioClip Slip;
    public AudioClip Skid;

    private AudioSource a;
    private WheelCollider c;
    private WheelHit hit;
    private bool isSkid = false;
    void Start()
    {
        a = GetComponent<AudioSource>();
        c = GetComponent<WheelCollider>();
    }


    void Update()
    {
        SetClip();
        if (!c.isGrounded)
        {
            a.volume = 0f;
            a.pitch = 1f;
            return;
        }
        c.GetGroundHit(out hit);
        float maxSlip = Mathf.Max(Mathf.Abs(hit.sidewaysSlip), Mathf.Abs(hit.forwardSlip));

        float clampMin = 0.02f;
        if (isSkid) clampMin = 0.04f;


        float value = Mathf.Clamp(maxSlip / 12f, clampMin, 0.08f);
        value -= clampMin;
        a.volume = Mathf.Lerp(a.volume, value, Time.deltaTime * 5f);
        a.pitch = Mathf.Lerp(a.pitch, Mathf.Clamp(value * 10f + 1f, 1f, 2f), Time.deltaTime * 5f);
    }

    private void SetClip()
    {
        if(c.brakeTorque == 0f && a.clip != Slip)
        {
            isSkid = false;
            a.clip = Slip;
            a.Play();
        }
        else if(c.brakeTorque != 0f && a.clip != Skid)
        {
            isSkid = true;
            a.clip = Skid;
            a.Play();
        }
    }
}
