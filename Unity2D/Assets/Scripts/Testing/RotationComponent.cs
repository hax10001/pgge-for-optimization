using UnityEngine;
using Unity.Entities;

public struct RotationComponent : IComponentData
{
    public Quaternion Value;
}
