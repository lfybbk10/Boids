using UnityEngine;
using Entitas;
using Unity.Mathematics;

namespace Boids.Entitas
{
    public class CalculateVelocitySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _boidsGroup;

        private readonly float _minSpeed;
        private readonly float _maxSpeed;

        public CalculateVelocitySystem(GameContext context, BoidSettings settings)
        {
            _boidsGroup = context.GetGroup(GameMatcher.AllOf(
                GameMatcher.Velocity,
                GameMatcher.Acceleration));

            _minSpeed = settings.minSpeed;
            _maxSpeed = settings.maxSpeed;
        }

        public void Execute()
        {
            foreach (GameEntity e in _boidsGroup)
            {
                e.velocity.value += e.acceleration.value * Time.deltaTime;

                float3 velocity = e.velocity.value;
                float speed = math.length(velocity);
                float3 dir = velocity / speed;
                speed = math.clamp(speed, _minSpeed, _maxSpeed);

                e.velocity.value = dir * speed;
            }
        }
    }
}