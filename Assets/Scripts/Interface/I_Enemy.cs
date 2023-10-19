using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Enemy 
{
    public string GetName();
    public void SetCenterPosition(Vector3 centerPosition);
    public void SetPlayer(Transform playerTransform);
    public void SetRoadLength(float length);
    public void Initialize();
    public void DestroyMe();
    public bool IsBomb();
    public void EndLife();
    public void RunBombView();
    public void SetParentUICanvas(Canvas parent);
    public void SetDefaultScale(Vector3 scale);
}
