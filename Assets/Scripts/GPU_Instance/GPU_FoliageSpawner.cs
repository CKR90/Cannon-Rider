using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GPU_FoliageSpawner : MonoBehaviour
{
    [SerializeField] private Transform Vehicle;
    [SerializeField] private List<GPU_FoliageType> FoliageTypes;

    [HideInInspector] public Foliage FoliageName;
    [HideInInspector] public bool StartSpawn = false;

    private bool init = false;
    private List<GPU_FoliageType> foliages;

    private List<List<GPUInstanceObjectData>> BatchesL = new List<List<GPUInstanceObjectData>>();
    private List<List<GPUInstanceObjectData>> BatchesR = new List<List<GPUInstanceObjectData>>();

    void Start()
    {
        //Temp - Test
        FoliageName = Foliage.Cactus;
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

        RenderBatches();


        for (int i = 0; i < FoliageTypes.Count; i++)
        {
            if (Vehicle.position.z - BatchesL[i][0].Position.z > foliages[i].distOfMeshes)
            {
                BatchesL[i][0].Position.z += foliages[i].Count * foliages[i].distOfMeshes;
                BatchesL[i].Add(BatchesL[i][0]);
                BatchesL[i].RemoveAt(0);
            }

            if (Vehicle.position.z - BatchesR[i][0].Position.z > foliages[i].distOfMeshes)
            {
                BatchesR[i][0].Position.z += foliages[i].Count * foliages[i].distOfMeshes;
                BatchesR[i].Add(BatchesR[i][0]);
                BatchesR[i].RemoveAt(0);
            }
        }
    }

    private void Initialize()
    {
        foliages = FoliageTypes.FindAll(x => x.FoliageName == FoliageName);



        foreach (var rock in foliages)
        {
            List<GPUInstanceObjectData> batchL = new List<GPUInstanceObjectData>();
            List<GPUInstanceObjectData> batchR = new List<GPUInstanceObjectData>();

            for (int i = 0; i < rock.Count; i++)
            {
                Vector3 pos = Vector3.zero;
                pos.x = Random.Range(rock.sideLimitMin, rock.sideLimitMax);
                pos.y = rock.YOffset;
                pos.z = rock.SpawnStartZ + rock.distOfMeshes * i;

                Vector3 euler = Vector3.zero;
                euler.y = Random.Range(0f, 360f);

                batchL.Add(new GPUInstanceObjectData(pos, rock.scale * Random.Range(1f, 1.2f), Quaternion.Euler(euler), null));

                pos.x = -Random.Range(rock.sideLimitMin, rock.sideLimitMax);
                euler.y = Random.Range(0f, 360f);

                batchR.Add(new GPUInstanceObjectData(pos, rock.scale * Random.Range(1f, 1.2f), Quaternion.Euler(euler), null));
            }

            BatchesL.Add(batchL);
            BatchesR.Add(batchR);
        }

    }
    private void RenderBatches()
    {
        for (int i = 0; i < BatchesL.Count; i++)
        {
            Graphics.DrawMeshInstanced(foliages[i].mesh, 0, foliages[i].material, BatchesL[i].Select((a) => a.Matrix).ToList());
            Graphics.DrawMeshInstanced(foliages[i].mesh, 0, foliages[i].material, BatchesR[i].Select((a) => a.Matrix).ToList());
        }

    }
}

[System.Serializable] public class GPU_FoliageType
{
    public string listName;
    public Foliage FoliageName;
    public Mesh mesh;
    public Material material;
    public float YOffset;
    public float SpawnStartZ;
    public Vector3 scale;
    public int Count = 300;
    [Min(0f)] public float sideLimitMin = 0f;
    [Min(0f)] public float sideLimitMax = 0f;
    [Min(1f)] public float distOfMeshes = 30f;

}