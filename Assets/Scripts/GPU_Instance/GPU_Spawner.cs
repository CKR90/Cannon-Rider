using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GPU_Spawner : MonoBehaviour
{

    [HideInInspector] public BasketData BasketData;
    [HideInInspector] public List<List<GPUInstanceObjectData>> Batches = new List<List<GPUInstanceObjectData>>();
    void Start()
    {
        if (!LevelDataTransfer.gamePlaySettings.basketEnable) return;

        for (int MatId = 0; MatId < BasketData.Materials.Count; MatId++)
        {
            List<GPUInstanceObjectData> batch = new List<GPUInstanceObjectData>();
            for (int i = MatId; i < BasketData.PositionTransforms.Count; i += BasketData.Materials.Count)
            {
                batch.Add(new GPUInstanceObjectData(BasketData.PositionTransforms[i].position, BasketData.MeshScale, Random.rotation, BasketData.PositionTransforms[i]));
            }
            Batches.Add(batch);
        }
        
    }
    void Update()
    {
        if (!LevelDataTransfer.gamePlaySettings.basketEnable) return;

        RenderBatches();
    }
    private void RenderBatches()
    {
        for (int MatId = 0; MatId < BasketData.Materials.Count; MatId++)
        {
            for (int i = 0; i < Batches[MatId].Count; i++) Batches[MatId][i].Position = BasketData.PositionTransforms[MatId + i * BasketData.Materials.Count].position;
            Graphics.DrawMeshInstanced(BasketData.Mesh, 0, BasketData.Materials[MatId], Batches[MatId].Select((a) => a.Matrix).ToList());
        }
    }
}

