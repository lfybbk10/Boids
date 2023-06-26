using Leopotam.EcsLite;
using Unity.Mathematics;

namespace Boids.LeoECS
{
    public class UpdatePositionSystem : IEcsRunSystem
    {
        private readonly EcsFilter _boidsGroup;
        private readonly EcsPool<Position> _positions;

        public UpdatePositionSystem(EcsWorld ecsWorld)
        {
            _boidsGroup = ecsWorld.Filter<Position>().End();
            _positions = ecsWorld.GetPool<Position>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int e in _boidsGroup)
            {
                ref Position position = ref _positions.Get(e);
                position.value += math.up();
            }
        }
    }
}