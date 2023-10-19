using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalController : MonoBehaviour
{
    public Transform Vehicle;
    public float maxSpawnDistance;
    public List<DecalType> Decals;

    [HideInInspector] public Decal DecalName;
    [HideInInspector] public bool StartSpawn = false;


    private bool init = false;
    private float spawnZ = 0f;

    private List<GameObject> SpawnedDecals = new List<GameObject>();

    private readonly float[] signNumbers = new float[2] {-1, 1};

    private int spawnedDecalIndex = 0;

    void Start()
    {
        DecalName = LevelDataTransfer.mapSettings.decalType;
        StartSpawn = true;
    }

    void Update()
    {
        if (!StartSpawn) return;

        if (spawnZ < Vehicle.position.z + maxSpawnDistance)
        {

            SpawnDecal(Decals.Find(x => x.DecalName == DecalName));
        }

        if(SpawnedDecals[0].transform.position.z < Vehicle.position.z - 20f)
        {
            Destroy(SpawnedDecals[0]);
            SpawnedDecals.RemoveAt(0);
        }
    }


    private void SpawnDecal(DecalType d)
    {
        spawnZ += Random.Range(30f, 100f);
        GameObject g = Instantiate(d.decals[Random.Range(0, d.decals.Count)], transform);
        SpawnedDecals.Add(g);

        g.name = "Decal " + spawnedDecalIndex.ToString();
        spawnedDecalIndex++;

        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(d.SideLimitMin, d.SideLimitMax) * signNumbers[Random.Range(0, signNumbers.Length)];
        pos.y = 0.001f;
        pos.z = spawnZ;

        g.transform.position = pos;

        if (d.randomRotation) g.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
    }

    public void ResetDecals()
    {
        foreach(var d in SpawnedDecals)
        {
            Destroy(d);
        }
        SpawnedDecals.Clear();
        spawnZ = 0f;

    }
}

