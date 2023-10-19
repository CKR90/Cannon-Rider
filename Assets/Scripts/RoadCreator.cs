using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoadCreator : MonoBehaviour
{
    public GameObject Vehicle;
    public GameObject RoadPart;
    public List<GameObject> Obstacles;
    public Slider DistanceSlider;
    public Slider ObstacleDensitySlider;
    public Transform ObstacleFolder;


    public float roadSizeX = 6f;
    public float roadSizeZ = 6f;

    private GameObject currentPart;
    private float? distance = 0f;

    private List<GameObject> parts = new List<GameObject>();

    private int EmptyObstacleRoadCount = 0;

    void Awake()
    {
        CreateRoad(true);
    }
    void Update()
    {
        try
        {
            CreateRoad();
            DestroyRoadParts();
        }
        catch { }
    }

    private void CreateRoad(bool createStartPart = false)
    {
        return;
        if(!createStartPart)
        {
            distance = GetDistance();
            if (distance != null && distance < DistanceSlider.value)
            {
                float z = 0f;
                if (currentPart != null)
                {
                    z = currentPart.transform.position.z + roadSizeZ;
                }

                currentPart = Instantiate(RoadPart, transform);
                currentPart.name = "RoadPart";
                currentPart.transform.position = new Vector3(0f, 0f, z);

                parts.Add(currentPart);

                int rand = Random.Range(0, (int)ObstacleDensitySlider.maxValue + 1 - (int)ObstacleDensitySlider.value);
                if (rand == 0) InstantiateObstacle();
            }
        }
        else
        {
            currentPart = Instantiate(RoadPart, transform);
            currentPart.name = "RoadPart";
            currentPart.transform.position = new Vector3(0f, 0f, 0f);
            parts.Add(currentPart);
        }
    }
    private float? GetDistance()
    {
        if (currentPart == null) return null;
        return Vector3.Distance(currentPart.transform.position, Vehicle.transform.position);
    }
    private void DestroyRoadParts()
    {
        for (int i = parts.Count - 1; i >= 0; i--)
        {
            if (parts[i] == null) parts.RemoveAt(i);
        }

        if (parts.Count == 0) return;

        GameObject old = parts[0];

        float distance = Vector3.Distance(Vehicle.transform.position, old.transform.position);

        if (distance > 20f) Destroy(old);
    }
    private void InstantiateObstacle()
    {
        if(Obstacles.Count == 0) return;

        Ray ray = new Ray(currentPart.transform.position + Vector3.up * 10f, -Vector3.up);
        if(Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            GameObject g = Instantiate(Obstacles[Random.Range(0, Obstacles.Count - 1)], ObstacleFolder);
            g.AddComponent<AutoKill>();
            g.transform.localScale = Vector3.one * .3f;
            g.name = "Enemy";
            g.tag = "Enemy";


            Vector3 pos = hit.point;
            pos.x = Random.Range(-roadSizeX, roadSizeX) / 2f;
            pos.z += Random.Range(-roadSizeZ, roadSizeZ) / 2f;

            Vector3 dir = (hit.point - pos).normalized;

            g.transform.position = pos;
            g.transform.forward = dir;
        }
        
    }

    public void ResetGame()
    {
        List<Transform> obs = ObstacleFolder.GetComponentsInChildren<Transform>().ToList();
        obs.RemoveAt(0);
        foreach(Transform t in obs)
        {
            if(t != null) DestroyImmediate(t.gameObject);
        }


        foreach(var v in parts)
        {
            DestroyImmediate(v);
        }
        parts.Clear();
        CreateRoad(true);
    }
}
