using UnityEngine;
using System.Collections;


public class Plane : MonoBehaviour
{

    public GameObject propBlured;

    void Update()
    {
        propBlured.transform.Rotate(1000 * Time.deltaTime, 0, 0);
    }
}