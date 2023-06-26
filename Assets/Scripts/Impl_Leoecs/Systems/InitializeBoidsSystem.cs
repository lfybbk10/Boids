using Leopotam.EcsLite;

namespace Boids.LeoECS
{
    public class InitializeBoidsSystem : IEcsInitSystem
    {
        private readonly EcsWorld _ecsWorld;

        private readonly int _boidCount;
        private readonly EcsPool<Position> positionPool;

        public InitializeBoidsSystem(EcsWorld ecsWorld, BoidSettings settings)
        {
            _ecsWorld = ecsWorld;
            _boidCount = settings.boidCount;
            positionPool = ecsWorld.GetPool<Position>();
        }
        
        public void Init(IEcsSystems systems)
        {
            for (int i = 0; i < _boidCount; i++)
            {
                int e = _ecsWorld.NewEntity();
                positionPool.Add(e);
            }
        }
    }
}