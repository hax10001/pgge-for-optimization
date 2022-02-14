using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public class SpawnTest : MonoBehaviour
{
    EntityManager manager;

    [SerializeField]
    BoxCollider2D Bounds;

    [SerializeField]
    private Mesh mesh;

    [SerializeField]
    private Material material;

    private void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        AddBoids(100000);
    }

    void AddBoids(int count)
    {
        NativeArray<Entity> entities = new NativeArray<Entity>(count, Allocator.Temp);

        EntityArchetype entityArchetype = manager.CreateArchetype(
            typeof(MaxSpeedComponent),
            typeof(SpeedComponent),
            typeof(PositionComponent),
            typeof(RotationComponent),
            typeof(RotationSpeedComponent),
            typeof(RenderBounds),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(Translation)
            );

        manager.CreateEntity(entityArchetype, entities);

        for (int i = 0; i < entities.Length; ++i)
        {
            Entity entity = entities[i];

            //float x = Random.Range(Bounds.bounds.min.x, Bounds.bounds.max.x);
            //float y = Random.Range(Bounds.bounds.min.y, Bounds.bounds.max.y);

            float randomX = UnityEngine.Random.Range(-10f, 10f);
            float randomY = UnityEngine.Random.Range(-4f, 4f);

            manager.SetComponentData(entity, new Translation
            {
                Value = new float3(randomX, randomY, 0f)
            });

            manager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = mesh,
                material = material
            });

            //AddBoid(x, y, flocks[0]);
        }

        entities.Dispose();
        //flocks[0].numBoids += count;
    }

    void AddBoid(float x, float y, Flock flock)
    {
        GameObject obj = Instantiate(flock.PrefabBoid);
        obj.name = "Boid_" + flock.name + "_" + flock.mAutonomous.Count;
        obj.transform.position = new Vector3(x, y, 0.0f);
        Autonomous boid = obj.GetComponent<Autonomous>();
        flock.mAutonomous.Add(boid);
        boid.MaxSpeed = flock.maxSpeed;
        boid.RotationSpeed = flock.maxRotationSpeed;
    }
}
