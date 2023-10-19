using UnityEngine;

public class GPUInstanceObjectData
{
    public Vector3 Position;
    public Vector3 Scale;
    public Quaternion Rotation;
    public Transform Referance;

    public GPUInstanceObjectData(Vector3 position, Vector3 scale, Quaternion rotation, Transform referance)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
        Referance = referance;
    }

    public Matrix4x4 Matrix
    {
        get
        {
            return Matrix4x4.TRS(Position, Rotation, Scale);
        }
    }
}
