using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    [SerializeField] private Transform Vehicle;
    [SerializeField] private float MaxSpawnDistance = 50f;
    [SerializeField] private float BackSideDestroyDistance = 10f;
    [SerializeField] private List<RockType> RockTypes;

    [HideInInspector] public Rock RockName;
    [HideInInspector] public bool StartSpawn = false;

    private List<GameObject> spawnedRocks = new List<GameObject>();

    private bool init = false;
    private RockType rocks;
    private float SpawnZ = 0f;
    void Start()
    {
        RockName = LevelDataTransfer.mapSettings.rockType;
        StartSpawn = true;
    }

    void Update()
    {
        if (!StartSpawn) return;

        if (!init)
        {
            init = true;
            Initialize();
        }

        if (SpawnZ < Vehicle.position.z + MaxSpawnDistance)
        {
            SpawnOnZline(SpawnZ);
            SpawnZ += 30f;
        }
        else
        {
            GameController.Instance.EnvironmentReady = true;
        }
    }
    private void Initialize()
    {
        rocks = RockTypes.Find(x => x.RockName == RockName);
        SpawnZ = Vehicle.position.z;
    }
    private void SpawnOnZline(float z)
    {
        int lineCount = 2;
        float sign = 1;
        for (int i = 0; i < lineCount; i++)
        {
            #region Initialize Foliage
            GameObject f = Instantiate(rocks.rocks[Random.Range(0, rocks.rocks.Count)], transform);
            spawnedRocks.Add(f);
            EnvironmentObjectDestroyer fd = f.AddComponent<EnvironmentObjectDestroyer>();
            fd.Vehicle = Vehicle;
            fd.BackSideDestroyDistance = BackSideDestroyDistance;
            #endregion
            #region Set Position
            Vector3 pos = Vector3.zero;

            
            pos.x = Random.Range(rocks.SideLimitMin, rocks.SideLimitMax) * sign;

            Ray ray = new Ray(new Vector3(pos.x, 100f, z), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, 1 << 9))
            {
                pos.y = hit.point.y;
            }

            pos.z = z;

            f.transform.position = pos;
            #endregion
            #region Rotation
            f.transform.localEulerAngles = (-transform.right * sign);
            f.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
            #endregion
            sign = -1;
        }
    }
    public void ResetRocks()
    {
        foreach(var f in spawnedRocks)
        {
            Destroy(f);
        }
        spawnedRocks.Clear();
        SpawnZ = 0f;
    }
}
