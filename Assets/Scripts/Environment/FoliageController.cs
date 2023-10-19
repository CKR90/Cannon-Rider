using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageController : MonoBehaviour
{
    [SerializeField] private Transform Vehicle;
    [SerializeField] private float MaxSpawnDistance = 50f;
    [SerializeField] private float BackSideDestroyDistance = 0f;
    [SerializeField] private List<FoliageType> FoliageTypes;

    [HideInInspector] public Foliage FoliageName;
    [HideInInspector] public bool StartSpawn = false;

    private bool init = false;

    private FoliageType foliages;
    private float SpawnZ = 0f;

    private readonly float[] numberSign = new float[2] { -1f, 1f };

    private List<GameObject> Items = new List<GameObject>();
    private int checkIndex = 0;


    private void Start()
    {
        FoliageName = LevelDataTransfer.mapSettings.foliageType;
        StartSpawn = true;
    }
    void FixedUpdate()
    {
        if (!StartSpawn) return;

        if(!init)
        {
            init = true;
            Initialize();
        }

        if(SpawnZ < Vehicle.position.z + MaxSpawnDistance)
        {
            SpawnOnZline(SpawnZ);
            SpawnZ += Random.Range(8f, 25f);
        }
        else
        {
            GameController.Instance.FoliageReady = true;
        }

        CheckItemDestroy();
    }

    private void SpawnOnZline(float z)
    {
        int lineCount = Random.Range(0, 5);

        for(int i = 0; i < lineCount; i++)
        {
            #region Initialize Foliage
            GameObject f = Instantiate(foliages.foliages[Random.Range(0, foliages.foliages.Count)], transform);
            Items.Add(f);
            #endregion
            #region Set Position
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(foliages.SideLimitMin, foliages.SideLimitMax) * numberSign[Random.Range(0, 2)];

            Ray ray = new Ray(new Vector3(pos.x, 100f, z), Vector3.down);
            if(Physics.Raycast(ray, out RaycastHit hit, 1000f, 1 << 9))
            {
                pos.y = hit.point.y;
            }

            pos.z = z;

            f.transform.position = pos;
            #endregion
            #region Rotation And Scale
            Vector3 euler = Vector3.zero;
            euler.y = Random.Range(0f, 360f);
            f.transform.localEulerAngles = euler;

            f.transform.localScale *= Random.Range(1f, 1.5f);
            #endregion
        }
    }
    private void Initialize()
    {
        foliages = FoliageTypes.Find(x => x.FoliageName == FoliageName);
        SpawnZ = Vehicle.position.z;
    }
    private void CheckItemDestroy()
    {
        if(Items.Count == 0) return;
        checkIndex = checkIndex % Items.Count;

        if(Vehicle.position.z - Items[checkIndex].transform.position.z > 30f)
        {
            Destroy(Items[checkIndex]);
            Items.RemoveAt(checkIndex);
        }
        else
        {
            checkIndex++;
        }
    }

    public void ResetFoliage()
    {
        foreach(var g in Items)
        {
            Destroy(g);
        }
        Items.Clear();
        SpawnZ = 0f;
    }
}

