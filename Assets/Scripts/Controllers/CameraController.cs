using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;

    public float focusSpeed = 1f;
    public float Distance = 2f;
    public float Height = 1f;
    public float LookHeight = .5f;
    void Start()
    {
        transform.position = GetPos();
        LookAtTarget();
    }

    
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, GetPos(), Time.deltaTime * focusSpeed);
        LookAtTarget();

    }

    private Vector3 GetPos()
    {
        return new Vector3(Target.position.x, Target.position.y + Height, Target.position.z - Distance);
    }
    private void LookAtTarget()
    {
        transform.LookAt(Target.position + Vector3.up * LookHeight);
    }

    public void ResetCamera()
    {
        transform.position = GetPos();
        LookAtTarget();
    }
}
