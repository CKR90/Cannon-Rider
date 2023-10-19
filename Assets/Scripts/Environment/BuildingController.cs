using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private Transform Vehicle;
    [SerializeField] private float MaxSpawnDistance = 50f;
    [SerializeField] private float BackSideDestroyDistance = 10f;
    [SerializeField] private List<BuildingType> BuildingTypes;

    [HideInInspector] public Building BuildingName;
    [HideInInspector] public bool StartSpawn = false;

    private bool init = false;

    private BuildingType buildings;
    private float SpawnZ = 0f;

    private readonly float[] numberSign = new float[2] { -1f, 1f };

    private List<GameObject> spawnedBuildings = new List<GameObject>();

    void Start()
    {
        BuildingName = LevelDataTransfer.mapSettings.buildingType;
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
            SpawnZ += Random.Range(150f, 300f);
        }
    }
    private void SpawnOnZline(float z)
    {
        int lineCount = Random.Range(0, 2);

        for (int i = 0; i < lineCount; i++)
        {
            #region Initialize Building
            GameObject f = Instantiate(buildings.buildings[Random.Range(0, buildings.buildings.Count)], transform);
            spawnedBuildings.Add(f);
            EnvironmentObjectDestroyer fd = f.AddComponent<EnvironmentObjectDestroyer>();
            fd.Vehicle = Vehicle;
            fd.BackSideDestroyDistance = BackSideDestroyDistance;
            #endregion
            #region Set Position
            Vector3 pos = Vector3.zero;

            float sign = numberSign[Random.Range(0, 2)];
            pos.x = Random.Range(buildings.SideLimitMin, buildings.SideLimitMax) * sign;

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
            f.transform.Rotate(Vector3.up, Random.Range(-20f, 20f));
            #endregion
        }
    }
    private void Initialize()
    {
        buildings = BuildingTypes.Find(x => x.BuildingName == BuildingName);
        SpawnZ = Vehicle.position.z;
    }
    public void ResetBuildings()
    {
        foreach(var b in spawnedBuildings)
        {
            Destroy(b);
        }
        spawnedBuildings.Clear();
        SpawnZ = 0f;
    }
}
