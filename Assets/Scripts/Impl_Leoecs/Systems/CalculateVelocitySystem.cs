using Leopotam.EcsLite;
using Unity.Mathematics;
using UnityEngine;

namespace Boids.LeoECS
{
    public class CalculateVelocitySystem : IEcsRunSystem
    {
        /*private readonly EcsFilter<Velocity, Acceleration> _boidsGroup;
        private readonly float _minSpeed;
        private readonly float _maxSpeed;
    
        public CalculateVelocitySystem(EcsWorld world, BoidSettings settings)
        {
            _boidsGroup = world.Filter<Velocity, Acceleration>().End();
            _minSpeed = settings.minSpeed;
            _maxSpeed = settings.maxSpeed;
        }
    
        public void Run(IEcsSystems systems)
        {
            float deltaTime = Time.deltaTime;
    
            foreach (int entityIndex in _boidsGroup)
            {
                ref Velocity velocity = ref _boidsGroup.Get1(entityIndex);
                ref Acceleration acceleration = ref _boidsGroup.Get2(entityIndex);
    
                velocity.Value += acceleration.Value * deltaTime;
    
                float3 currentVelocity = velocity.Value;
                float speed = math.length(currentVelocity);
                float3 direction = currentVelocity / speed;
                speed = math.clamp(speed, _minSpeed, _maxSpeed);
    
                velocity.Value = direction * speed;
            }
        }*/
        public void Run(IEcsSystems systems)
        {
        }
    }
}