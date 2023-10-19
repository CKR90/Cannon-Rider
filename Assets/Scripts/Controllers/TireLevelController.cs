using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireLevelController : MonoBehaviour
{
    public WheelCollider L;
    public WheelCollider R;

    public int tireLevel = 0;
    public int suspensionLevel = 0;
    void Start()
    {
        SetVehicleProperty();
    }
    private void SetVehicleProperty()
    {
        tireLevel = Mathf.Clamp(User.info.tirelevel, 1, 20);
        suspensionLevel = Mathf.Clamp(User.info.suspensionlevel, 1, 20);

        WheelFrictionCurve fl = L.forwardFriction;
        WheelFrictionCurve fr = R.forwardFriction;

        WheelFrictionCurve sl = L.sidewaysFriction;
        WheelFrictionCurve sr = R.sidewaysFriction;

        JointSpring susl = L.suspensionSpring;
        JointSpring susr = R.suspensionSpring;

        switch (tireLevel)
        {
            case 1:  fl.extremumValue = 1f;    fr.extremumValue = 1f;    sl.extremumValue = 1f;   sr.extremumValue = 1f;   break;
            case 2:  fl.extremumValue = 1.1f;  fr.extremumValue = 1.1f;  sl.extremumValue = 1.2f; sr.extremumValue = 1.2f; break;
            case 3:  fl.extremumValue = 1.15f; fr.extremumValue = 1.15f; sl.extremumValue = 1.3f; sr.extremumValue = 1.3f; break;
            case 4:  fl.extremumValue = 1.2f;  fr.extremumValue = 1.2f;  sl.extremumValue = 1.4f; sr.extremumValue = 1.4f; break;
            case 5:  fl.extremumValue = 1.25f; fr.extremumValue = 1.25f; sl.extremumValue = 1.5f; sr.extremumValue = 1.5f; break;
            case 6:  fl.extremumValue = 1.3f;  fr.extremumValue = 1.3f;  sl.extremumValue = 1.6f; sr.extremumValue = 1.6f; break;
            case 7:  fl.extremumValue = 1.35f; fr.extremumValue = 1.35f; sl.extremumValue = 1.7f; sr.extremumValue = 1.7f; break;
            case 8:  fl.extremumValue = 1.4f;  fr.extremumValue = 1.4f;  sl.extremumValue = 1.8f; sr.extremumValue = 1.8f; break;
            case 9:  fl.extremumValue = 1.45f; fr.extremumValue = 1.45f; sl.extremumValue = 1.9f; sr.extremumValue = 1.9f; break;
            case 10: fl.extremumValue = 1.5f;  fr.extremumValue = 1.5f;  sl.extremumValue = 2.0f; sr.extremumValue = 2.0f; break;
            case 11: fl.extremumValue = 1.55f; fr.extremumValue = 1.55f; sl.extremumValue = 2.1f; sr.extremumValue = 2.1f; break;
            case 12: fl.extremumValue = 1.6f;  fr.extremumValue = 1.6f;  sl.extremumValue = 2.2f; sr.extremumValue = 2.2f; break;
            case 13: fl.extremumValue = 1.65f; fr.extremumValue = 1.65f; sl.extremumValue = 2.3f; sr.extremumValue = 2.3f; break;
            case 14: fl.extremumValue = 1.7f;  fr.extremumValue = 1.7f;  sl.extremumValue = 2.4f; sr.extremumValue = 2.4f; break;
            case 15: fl.extremumValue = 1.75f; fr.extremumValue = 1.75f; sl.extremumValue = 2.5f; sr.extremumValue = 2.5f; break;
            case 16: fl.extremumValue = 1.8f;  fr.extremumValue = 1.8f;  sl.extremumValue = 2.6f; sr.extremumValue = 2.6f; break;
            case 17: fl.extremumValue = 1.85f; fr.extremumValue = 1.85f; sl.extremumValue = 2.7f; sr.extremumValue = 2.7f; break;
            case 18: fl.extremumValue = 1.9f;  fr.extremumValue = 1.9f;  sl.extremumValue = 2.8f; sr.extremumValue = 2.8f; break;
            case 19: fl.extremumValue = 1.95f; fr.extremumValue = 1.95f; sl.extremumValue = 2.9f; sr.extremumValue = 2.9f; break;
            case 20: fl.extremumValue = 2f;    fr.extremumValue = 2f;    sl.extremumValue = 3f;   sr.extremumValue = 3f;   break;
        }

        switch (suspensionLevel)
        {
            case 1:  susl.spring = 200000f; susr.spring = 200000f; break;
            case 2:  susl.spring = 190000f; susr.spring = 190000f; break;
            case 3:  susl.spring = 180000f; susr.spring = 180000f; break;
            case 4:  susl.spring = 170000f; susr.spring = 170000f; break;
            case 5:  susl.spring = 160000f; susr.spring = 160000f; break;
            case 6:  susl.spring = 150000f; susr.spring = 150000f; break;
            case 7:  susl.spring = 140000f; susr.spring = 140000f; break;
            case 8:  susl.spring = 130000f; susr.spring = 130000f; break;
            case 9:  susl.spring = 120000f; susr.spring = 120000f; break;
            case 10: susl.spring = 110000f; susr.spring = 110000f; break;
            case 11: susl.spring = 105000f; susr.spring = 105000f; break;
            case 12: susl.spring = 100000f; susr.spring = 100000f; break;
            case 13: susl.spring = 90000f;  susr.spring = 90000f;  break;
            case 14: susl.spring = 80000f;  susr.spring = 80000f;  break;
            case 15: susl.spring = 70000f;  susr.spring = 70000f;  break;
            case 16: susl.spring = 60000f;  susr.spring = 60000f;  break;
            case 17: susl.spring = 55000f;  susr.spring = 55000f;  break;
            case 18: susl.spring = 50000f;  susr.spring = 50000f;  break;
            case 19: susl.spring = 45000f;  susr.spring = 45000f;  break;
            case 20: susl.spring = 40000f;  susr.spring = 40000f;  break;
        }

        L.forwardFriction  = fl;
        L.sidewaysFriction = sl;
        L.suspensionSpring = susl;

        R.forwardFriction  = fr;
        R.sidewaysFriction = sr;
        R.suspensionSpring = susr;
    }
}
