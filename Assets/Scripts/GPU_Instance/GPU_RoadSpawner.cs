using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GPU_RoadSpawner : MonoBehaviour
{
    public string Name = "";
    public int Count = 300;
    public float xPosition = 0f;
    public float distanceOffset = 0f;
    public bool createCollider = true;
    public float ColliderLength = 18f;
    public Mesh ObjectMesh;
    public Vector3 ObjectScale = new Vector3(12f, 0.2f, 6f);
    public Material ObjectMaterial;
    public Transform Vehicle;

    private List<GPUInstanceObjectData> Batches = new List<GPUInstanceObjectData>();
    private GameObject col;
    void Start()
    {
        for (int i = 0; i < Count; i++)
        {
            Vector3 pos = Vector3.forward * i * (ObjectScale.z + distanceOffset) + Vector3.right * xPosition;
            Batches.Add(new GPUInstanceObjectData(pos, ObjectScale, Quaternion.identity, null));
        }
        if(createCollider)
        {
            col = new GameObject("Road Collider");
            col.tag = "Road";
            col.layer = 9;
            col.AddComponent<BoxCollider>();
            col.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, ObjectScale.z * Count / 2f - ObjectScale.z / 2f);
            col.GetComponent<BoxCollider>().size = new Vector3(ColliderLength, ObjectScale.y, ObjectScale.z * Count);
        }
        else
        {
            col = new GameObject(Name);
            col.transform.SetParent(transform);
        }
    }


    void Update()
    {
        RenderBatches();

        if (Vehicle.position.z - col.transform.position.z > (ObjectScale.z + distanceOffset) * 3f)
        {
            AddZ(ObjectScale.z + distanceOffset);

            Batches[0].Position.z = Batches[Batches.Count - 1].Position.z + ObjectScale.z + distanceOffset;
            Batches.Add(Batches[0]);
            Batches.RemoveAt(0);
        }
    }
    private void RenderBatches()
    {
        Graphics.DrawMeshInstanced(ObjectMesh, 0, ObjectMaterial, Batches.Select((a) => a.Matrix).ToList());
    }
    private void AddZ(float value)
    {
        Vector3 pos = col.transform.position;
        pos.z += value;
        col.transform.position = pos;
    }

    public void ResetRoad()
    {
        for (int i = 0; i < Count; i++)
        {
            Batches[i].Position.z = i * (ObjectScale.z + distanceOffset);
        }
        col.transform.position = Vector3.zero;
    }
}
