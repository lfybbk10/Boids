using Boids.LeoECS;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration;
using UnityEngine;

public class LeoEcsGameController : MonoBehaviour
{
    [SerializeField] private BoidSettings boidSettings = null;
    
    private EcsWorld _world;
    private IEcsSystems _systems;

    private void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        
        _systems
            .Add(new InitializeBoidsSystem(_world, boidSettings))
            .Add(new UpdatePositionSystem(_world))
            
#if UNITY_EDITOR
            .Add(new EcsWorldDebugSystem())
#endif
            .Init();
    }
        
    private void Update()
    {
        _systems.Run();
    }
}
