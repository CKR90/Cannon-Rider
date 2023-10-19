using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TrajectoryVisualizer : MonoBehaviour
{
    public Transform line;
    public MeshRenderer lineRenderer;
    public Material FocusedLine;
    public Material UnfocusedLine;
    public GameObject Crosshair;
    private bool mouseDrag = false;

    private RaycastHit hit;
    private bool detect = false;

    private bool finishGame = false;

    private void Awake()
    {
        line.gameObject.SetActive(false);
        Crosshair.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (finishGame) return;

        if (Input.GetMouseButton(0) && !mouseDrag && GameAreaActivator.Instance.ControlEnabled)
        {
            if (!GetComponent<CannonGun>().controllerType)
            {
                mouseDrag = false;
                return;
            }

            mouseDrag = true;
            _OnMouseDown();
        }
        else if(Input.GetMouseButton(0) && mouseDrag && GameAreaActivator.Instance.ControlEnabled)
        {
            _OnMouseDrag();
        }
        else if (!Input.GetMouseButton(0) && mouseDrag)
        {
            mouseDrag = false;
            _OnMouseUp();
        }
    }

    private void _OnMouseDown() 
    {
        line.gameObject.SetActive(true);


    }
    private void _OnMouseDrag() 
    {
        float dist = GetComponent<CannonGun>().BulletImpactDistance;

        Ray ray = new Ray(line.position, line.forward);
        Vector3 hitPoint = Vector3.zero;
        if(Physics.Raycast(ray, out hit, GetComponent<CannonGun>().BulletImpactDistance))
        {
            if(hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "TNT" || hit.collider.gameObject.tag == "AwardBox")
            {
                detect = true;
                dist = hit.distance;
                Crosshair.SetActive(true);
                hitPoint = hit.point;
                if (lineRenderer.sharedMaterial != FocusedLine) lineRenderer.sharedMaterial = FocusedLine;
            }
            else
            {
                detect = false;
                Crosshair.SetActive(false);
                if (lineRenderer.sharedMaterial != UnfocusedLine) lineRenderer.sharedMaterial = UnfocusedLine;
            }
        }
        else
        {
            detect = false;
            Crosshair.SetActive(false);
            if (lineRenderer.sharedMaterial != UnfocusedLine) lineRenderer.sharedMaterial = UnfocusedLine;
        }
        Vector3 scale = line.localScale;
        scale.z = dist;
        line.localScale = scale;
        Crosshair.transform.position = hitPoint + (transform.position - Crosshair.transform.position).normalized * .5f;
    }
    private void _OnMouseUp() 
    {
        line.gameObject.SetActive(false);
        Crosshair.SetActive(false);
        lineRenderer.sharedMaterial = UnfocusedLine;

        if(!finishGame)
        {
            if (detect) GetComponent<CannonGun>().Fire(hit);
            else GetComponent<CannonGun>().Fire();
        }
    }

    public void FinishGame()
    {
        finishGame = true;
        _OnMouseUp();
    }

}
